using System;
using System.IO;
using System.Linq;
using Digger.Unsafe;
using Unity.Collections;
using UnityEngine;

namespace Digger
{
    public static class Upgrading2
    {
        public static bool HasLegacyFiles(DiggerSystem diggerSystem)
        {
            var basePath = diggerSystem.BasePathData;
            var internalPath = Path.Combine(basePath, ".internal");
            var internalDir = new DirectoryInfo(internalPath);

            if (!internalDir.Exists) {
                return false;
            }

            return !internalDir.EnumerateFiles($"*.{DiggerSystem.VoxelFileExtension}").Select(info => info.Extension == $".{DiggerSystem.VoxelFileExtension}").Any() &&
                   (internalDir.EnumerateFiles($"*.{DiggerSystem.VoxelFileExtensionLegacyV2}").Select(info => info.Extension == $".{DiggerSystem.VoxelFileExtensionLegacyV2}")
                               .Any() ||
                    internalDir.EnumerateFiles($"*.{DiggerSystem.VoxelFileExtensionLegacyV2}_v*")
                               .Select(info => info.Extension.StartsWith($".{DiggerSystem.VoxelFileExtensionLegacyV2}_v")).Any());
        }

        public static void UpgradeDiggerData(DiggerSystem diggerSystem)
        {
            var basePath = diggerSystem.BasePathData;
            var internalPath = Path.Combine(basePath, ".internal");
            var internalDir = new DirectoryInfo(internalPath);

            if (!internalDir.Exists) {
                Debug.Log(
                    $"No DiggerData found for '{diggerSystem.Terrain.name}' at '{internalDir.FullName}'. Nothing to upgrade.");
                return;
            }

            if (!HasLegacyFiles(diggerSystem))
                return;

            Debug.Log($"Legacy Digger files (vox2) detected for terrain '{diggerSystem.Terrain.name}' at '{internalDir.FullName}'. Upgrading...");
            BackupDirectory(new DirectoryInfo(basePath));

            foreach (var file in internalDir.GetFiles($"*.{DiggerSystem.VoxelFileExtensionLegacyV2}")) {
                if (file.Extension != $".{DiggerSystem.VoxelFileExtensionLegacyV2}")
                    continue;

                var newPath = file.FullName.Replace($".{DiggerSystem.VoxelFileExtensionLegacyV2}", $".{DiggerSystem.VoxelFileExtension}");
                var rawBytes = File.ReadAllBytes(file.FullName);
                Voxel[] voxelArray = null;
                UpgradeVoxels(diggerSystem.SizeVox, rawBytes, ref voxelArray);
                PersistUpgradedVoxels(newPath, voxelArray);
                file.Delete();
                Debug.Log($"Upgraded file '{file.FullName}' to '{newPath}'");
            }

            foreach (var file in internalDir.GetFiles($"*.{DiggerSystem.VoxelFileExtensionLegacyV2}_v*")) {
                if (!file.Extension.StartsWith($".{DiggerSystem.VoxelFileExtensionLegacyV2}_v"))
                    continue;

                var newPath = file.FullName.Replace($".{DiggerSystem.VoxelFileExtensionLegacyV2}_v", $".{DiggerSystem.VoxelFileExtension}_v");
                var rawBytes = File.ReadAllBytes(file.FullName);
                Voxel[] voxelArray = null;
                UpgradeVoxels(diggerSystem.SizeVox, rawBytes, ref voxelArray);
                PersistUpgradedVoxels(newPath, voxelArray);
                file.Delete();
                Debug.Log($"Upgraded file '{file.FullName}' to '{newPath}'");
            }
        }

        private static void UpgradeVoxels(int newSizeVox, byte[] rawBytes, ref Voxel[] voxelArray)
        {
            var oldSizeVox = newSizeVox + 1;
            Voxel[] legacyVoxelArray = null;
            ReadLegacyVoxelBytes(oldSizeVox, rawBytes, ref legacyVoxelArray);

            if (voxelArray == null)
                voxelArray = new Voxel[newSizeVox * newSizeVox * newSizeVox];

            var oldSizeVox2 = oldSizeVox * oldSizeVox;
            for (var i = 0; i < legacyVoxelArray.Length; i++) {
                var xi = i / oldSizeVox2;
                var yi = (i - xi * oldSizeVox2) / oldSizeVox;
                var zi = i - xi * oldSizeVox2 - yi * oldSizeVox;
                if (xi == 0 || yi == 0 || zi == 0)
                    continue;

                voxelArray[(xi - 1) * newSizeVox * newSizeVox + (yi - 1) * newSizeVox + (zi - 1)] = legacyVoxelArray[i];
            }
        }

        private static void PersistUpgradedVoxels(string path, Voxel[] voxelArray)
        {
            var voxels = new NativeArray<Voxel>(voxelArray, Allocator.Temp);
            var bytes = new NativeSlice<Voxel>(voxels).SliceConvert<byte>();
            File.WriteAllBytes(path, bytes.ToArray());
            voxels.Dispose();
        }

        private static void BackupDirectory(DirectoryInfo sourcePath)
        {
            var projectDir = new DirectoryInfo(Application.dataPath).Parent;
            if (projectDir == null || !projectDir.Exists)
                return;

            Debug.Log($"Project directory = '{projectDir.FullName}'");
            var relDir = sourcePath.FullName.Replace(projectDir.FullName, "").Trim('/').Trim('\\').Trim(Path.PathSeparator);
            Debug.Log($"Relative directory = '{relDir}'");
            var destinationPath = projectDir.CreateSubdirectory("DiggerBackup").CreateSubdirectory(relDir);

            Debug.Log($"Backuping '{sourcePath.FullName}' directory to '{destinationPath.FullName}'");

            //Now Create all of the directories
            foreach (var dirPath in Directory.GetDirectories(sourcePath.FullName, "*",
                                                             SearchOption.AllDirectories))
                Directory.CreateDirectory(dirPath.Replace(sourcePath.FullName, destinationPath.FullName));

            //Copy all the files & Replaces any files with the same name
            foreach (var newPath in Directory.GetFiles(sourcePath.FullName, "*.*",
                                                       SearchOption.AllDirectories))
                File.Copy(newPath, newPath.Replace(sourcePath.FullName, destinationPath.FullName), true);
        }

        private static void ReadLegacyVoxelBytes(int sizeVox, byte[] rawBytes, ref Voxel[] voxelArray)
        {
            if (voxelArray == null)
                voxelArray = new Voxel[sizeVox * sizeVox * sizeVox];

            var voxelBytes = new NativeArray<byte>(rawBytes, Allocator.Temp);
            var bytes = new NativeSlice<byte>(voxelBytes);
            var voxelSlice = bytes.SliceConvert<Voxel>();
            DirectNativeCollectionsAccess.CopyTo(voxelSlice, voxelArray);
            voxelBytes.Dispose();
        }
    }
}