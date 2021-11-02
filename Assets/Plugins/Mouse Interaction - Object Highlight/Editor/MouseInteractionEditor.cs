using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(MouseInteraction))]
public class MouseInteractionEditor : Editor {

  public enum InteractionMode { Mouse, CenterOfTheScreen, TouchClick }
  public InteractionMode interactionMode;

  public override void OnInspectorGUI() {

    MouseInteraction myMouseInteraction = (MouseInteraction)target;

    if (!myMouseInteraction.useCenterScreen && !myMouseInteraction.useTouchClick) {
      interactionMode = InteractionMode.Mouse;
    } else if (!myMouseInteraction.useTouchClick) {
      interactionMode = InteractionMode.CenterOfTheScreen;
    } else {
      interactionMode = InteractionMode.TouchClick;
    }

    // Settings of the Mouse Interaction script

    EditorGUILayout.Separator(); // Blank Line (space separator)

    // Interaction Color
    myMouseInteraction.interactionColor = EditorGUILayout.ColorField(new GUIContent("Interaction Color", "Color change of the interacted object."), myMouseInteraction.interactionColor);

    // Interaction Speed
    myMouseInteraction.interactionSpeed = EditorGUILayout.FloatField(new GUIContent("Interaction Speed", "Fade speed of the color change (slow -> quick)"), myMouseInteraction.interactionSpeed);

    // Emission Intensity
    myMouseInteraction.emissionIntensity = EditorGUILayout.Slider(new GUIContent("Emission Intensity", "Emission intensity (doesn't work with material which has no emissive intensity)"), myMouseInteraction.emissionIntensity, 0.0f, 1.0f);

    EditorGUILayout.Separator(); // Blank Line (space separator)

    // Interaction Mode
    interactionMode = (InteractionMode)EditorGUILayout.EnumPopup(new GUIContent("Interaction Mode", "How the interaction work (mouse over, center of the screen or simple touch/click)"), interactionMode);
    switch (interactionMode) {
      case InteractionMode.Mouse:
        myMouseInteraction.useCenterScreen = false;
        myMouseInteraction.useTouchClick = false;
        break;
      case InteractionMode.CenterOfTheScreen:
        myMouseInteraction.useCenterScreen = true;
        myMouseInteraction.useTouchClick = false;
        break;
      case InteractionMode.TouchClick:
        myMouseInteraction.useCenterScreen = false;
        myMouseInteraction.useTouchClick = true;
        break;
      default:
        break;
    }

    // Hold Mouse Interaction
    if (myMouseInteraction.useTouchClick || myMouseInteraction.useCenterScreen)
      GUI.enabled = false;
    myMouseInteraction.holdMouseInteraction = EditorGUILayout.Toggle(new GUIContent("Hold Mouse Interaction", "Hold the mouse interaction when clicking on the object (Mouse mode only)"), myMouseInteraction.holdMouseInteraction);
    GUI.enabled = true;

    // Mouse Cursor
    if (interactionMode != InteractionMode.Mouse)
      GUI.enabled = false;
    EditorGUILayout.BeginHorizontal();
    EditorGUILayout.PrefixLabel(new GUIContent("Mouse Cursor", "Cursor sprite when interaction with the object (texture must be a Cursor in import settings)"));
    myMouseInteraction.mouseCursor = (Texture2D)EditorGUILayout.ObjectField(myMouseInteraction.mouseCursor, typeof(Texture2D), false);
    EditorGUILayout.EndHorizontal();
    GUI.enabled = true;

    // Interaction distance
    myMouseInteraction.interactionDistance = EditorGUILayout.IntField(new GUIContent("Interaction Distance", "Max distance of the interaction (1000 = far away, 6 = melee range)"), myMouseInteraction.interactionDistance);

    // Grouped Interaction
    myMouseInteraction.groupedInteraction = EditorGUILayout.Toggle(new GUIContent("Grouped Interaction", "Interaction with all objects of the same parent."), myMouseInteraction.groupedInteraction);

    // Number Of Ascent
    if (!myMouseInteraction.groupedInteraction)
      GUI.enabled = false;
    myMouseInteraction.numberOfAscent = EditorGUILayout.IntSlider(new GUIContent("Number Of Ascent", "Number of ascent to define the parent for the Grouped Interaction setting."), myMouseInteraction.numberOfAscent, 0, 4);
    GUI.enabled = true;

    // Interaction Animation
    myMouseInteraction.interactionAnim = (AnimationClip)EditorGUILayout.ObjectField(new GUIContent("Interaction Animation", "Animation played when interacted."), myMouseInteraction.interactionAnim, typeof(AnimationClip), false);
    if (myMouseInteraction.interactionAnim == null)
      GUI.enabled = false;

    // Animation Loop
    myMouseInteraction.animationLoop = EditorGUILayout.Toggle(new GUIContent("Animation Loop", "Loop the interacted animation."), myMouseInteraction.animationLoop);

    // Animation Reset
    if (!myMouseInteraction.animationLoop)
      GUI.enabled = false;
    myMouseInteraction.animationReset = EditorGUILayout.Toggle(new GUIContent("Animation Reset", "[Loop animation only] Reset the animation loop when the interaction exit."), myMouseInteraction.animationReset);
    GUI.enabled = true;

    EditorGUILayout.Separator(); // Blank Line (space separator)

    // Show Tooltip (Beginning of a group list where settings are disabled if you don't want to show tooltip)
    myMouseInteraction.showTooltip = EditorGUILayout.BeginToggleGroup(new GUIContent("Show Tooltip", "Show a text over the interacted object."), myMouseInteraction.showTooltip);

    EditorGUI.indentLevel++;

    // Tooltip UI Panel
    myMouseInteraction.tooltipUIPanel = (GameObject)EditorGUILayout.ObjectField(new GUIContent("Tooltip UI Panel", "Show a predefined UI Panel over the interacted object."), myMouseInteraction.tooltipUIPanel, typeof(GameObject), true);

    // Fixed to the Object
    myMouseInteraction.fixedToTheObject = EditorGUILayout.Toggle(new GUIContent("Fixed to the Object", "Show the tooltip over the object instead of over the mouse."), myMouseInteraction.fixedToTheObject);

    // Don't Exit Interaction On Clicking UI
    if (myMouseInteraction.useCenterScreen || (!myMouseInteraction.useCenterScreen && !myMouseInteraction.useTouchClick && !myMouseInteraction.holdMouseInteraction))
      GUI.enabled = false;
    myMouseInteraction.dontExitInteractionOnClickingUI = EditorGUILayout.Toggle(new GUIContent("Don't Exit Interaction On Clicking UI", "Don't exit interaction when clicking on UI element (only available on Touch/Click mode and Mouse mode with Hold Mouse Interaction setting)"), myMouseInteraction.dontExitInteractionOnClickingUI);
    if (myMouseInteraction.showTooltip)
      GUI.enabled = true;

    // Tooltip Position
    myMouseInteraction.tooltipPosition = EditorGUILayout.Vector2Field(new GUIContent("Tooltip Position", "Position of the tooltip showed over the interacted object."), myMouseInteraction.tooltipPosition);

    // Tooltip Text
    myMouseInteraction.tooltipText = EditorGUILayout.TextField(new GUIContent("Tooltip Text", "Text to show over the interacted object."), myMouseInteraction.tooltipText);

    if (myMouseInteraction.tooltipText == "")
      GUI.enabled = false;
    EditorGUI.indentLevel++;

    // Tooltip Text Color
    myMouseInteraction.tooltipColor = EditorGUILayout.ColorField(new GUIContent("Text Color", "[Requires Tooltip Text] Color of the text showed over the interacted object."), myMouseInteraction.tooltipColor);

    // Tooltip Text Size
    myMouseInteraction.tooltipSize = EditorGUILayout.IntField(new GUIContent("Text Size", "[Requires Tooltip Text] Size of the text showed over the interacted object."), myMouseInteraction.tooltipSize);

    // Tooltip Text Scaled
    myMouseInteraction.textResized = EditorGUILayout.Toggle(new GUIContent("Text Resized", "Resize the text, relative to the distance between the object and the camera."), myMouseInteraction.textResized);

    // Tooltip Text Font
    myMouseInteraction.tooltipFont = (Font)EditorGUILayout.ObjectField(new GUIContent("Text Font", "[Requires Tooltip Text] Font of the text showed over the interacted object."), myMouseInteraction.tooltipFont, typeof(Font), false);

    // Tooltip Alignment
    myMouseInteraction.tooltipAlignment = (MouseInteraction.TooltipAlignment)EditorGUILayout.EnumPopup(new GUIContent("Text Alignment", "Alignment of the text showed over the interacted object."), myMouseInteraction.tooltipAlignment);

    // Tooltip Shadow Color
    myMouseInteraction.tooltipShadowColor = EditorGUILayout.ColorField(new GUIContent("Text Shadow Color", "Color of the text shadow showed over the interacted object."), myMouseInteraction.tooltipShadowColor);

    // Tooltip Shadow Position	
    myMouseInteraction.tooltipShadowPosition = EditorGUILayout.Vector2Field(new GUIContent("Text Shadow Position", "Position of the text shadow showed over the interacted object."), myMouseInteraction.tooltipShadowPosition);

    EditorGUI.indentLevel--;
    GUI.enabled = true;
    EditorGUI.indentLevel--;

    EditorGUILayout.EndToggleGroup(); // End of the group list (of Show Tooltip)

    EditorGUILayout.Separator(); // Blank Line (space separator)

    // Using Event (Beginning of a group list where settings are disabled if you don't want to use event)
    myMouseInteraction.usingEvent = EditorGUILayout.BeginToggleGroup(new GUIContent("Using Event", "Enable event options for calling external method."), myMouseInteraction.usingEvent);
    if (myMouseInteraction.usingEvent) {
      SerializedProperty sprop;
      // Event called when object enter in interacted mode
      sprop = serializedObject.FindProperty("eventInteractionEnter");
      EditorGUILayout.PropertyField(sprop);
      serializedObject.ApplyModifiedProperties();
      // Event called when object exit the interacted mode
      sprop = serializedObject.FindProperty("eventInteractionExit");
      EditorGUILayout.PropertyField(sprop);
      serializedObject.ApplyModifiedProperties();
    }
    EditorGUILayout.EndToggleGroup(); // End of the group list (of Using Event)

    EditorGUILayout.Separator(); // Blank Line (space separator)

    // Warning messages
    // Check if the interacted object has a collider
    if (!myMouseInteraction.transform.gameObject.GetComponent<Collider>())
      EditorGUILayout.HelpBox("Don't forget to attach a collider to this object.", MessageType.Warning);
    // Advice for the Touch/Click interaction
    if (myMouseInteraction.useTouchClick && !myMouseInteraction.fixedToTheObject && myMouseInteraction.showTooltip)
      EditorGUILayout.HelpBox("In Touch/Click interaction, 'Fixed to the object' setting is recommended for mobile uses.", MessageType.Warning);
    // Check if the object has an animation component for the Interaction Animation setting
    if (myMouseInteraction.interactionAnim != null && myMouseInteraction.GetComponent<Animation>() == null)
      EditorGUILayout.HelpBox("There is no Animation Component attached to this object.", MessageType.Error);
    // Check if the object has the good number of parent, related to the Number Of Ascent setting
    if (myMouseInteraction.groupedInteraction) {
      bool error_parent = false;
      Transform current_parent = myMouseInteraction.transform.parent;
      for (int i = 1; i <= myMouseInteraction.numberOfAscent; i++) {
        if (current_parent == null) {
          error_parent = true;
          break;
        } else {
          current_parent = current_parent.parent;
        }
      }
      if (error_parent)
        EditorGUILayout.HelpBox("Number Of Ascent value is too high, this object doesn't have enough parent.", MessageType.Error);
    }
    // Check if the UI Panel has a RectTransform component
    /*if(myMouseInteraction.checkOutOfScreen && myMouseInteraction.tooltipUIPanel != null) {
              if(myMouseInteraction.tooltipUIPanel.GetComponent<RectTransform>() == null)
                        EditorGUILayout.HelpBox("Your UI Panel must have a RectTransform component.", MessageType.Error) ;
    }*/


  }

}
