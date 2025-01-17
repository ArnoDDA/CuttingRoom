﻿using System;
using UnityEngine;
using UnityEditor;
using CuttingRoom;
using CuttingRoom.VariableSystem;
using System.Threading.Tasks;

namespace CuttingRoom.Editor
{
	public static class CuttingRoomContextMenus
	{
		private const int menuItemPriority = 0;

		//[MenuItem("Cutting Room/Scene Utilities/Set All Atomic Narrative Object Durations To Length Of Media Source")]
		//public static void SetAllAtomsDurationToMediaLength()
		//{
		//	if (EditorUtility.DisplayDialog("Warning", "Are you sure you want to set all Atomic Narrative Objects to the length of their assigned Media Source?", "Yes", "Cancel"))
		//	{
		//		CuttingRoomUtilities.SetAllSceneAtomsDurationToMediaLength();
		//	}
		//}

		[MenuItem("GameObject/Cutting Room/Create/Narrative Space", false, menuItemPriority)]
		public static NarrativeSpace CreateNarrativeSpace()
		{
			NarrativeSpace narrativeSpace = new GameObject("NarrativeSpace", typeof(NarrativeSpace)).GetComponent<NarrativeSpace>();

			// Set reference to variable store.
			narrativeSpace.GlobalVariableStore = narrativeSpace.GetComponent<VariableStore>();

			return narrativeSpace;
		}

		public static Sequencer CreateSequencer()
		{
			Sequencer sequencer = new GameObject("Sequencer", typeof(Sequencer)).GetComponent<Sequencer>();

			// Find narrative space and try to link it to the sequencer.
			NarrativeSpace narrativeSpace = GameObject.FindObjectOfType<NarrativeSpace>();

			if (narrativeSpace != null)
			{
				sequencer.NarrativeSpace = narrativeSpace;
			}

			return sequencer;
		}

		public static AtomicNarrativeObject InstantiateAtomicNarrativeObject()
		{
			// Create the game object.
			GameObject atomicNarrativeObjectGO = new GameObject("AtomicNarrativeObject", typeof(AtomicNarrativeObject));

			Undo.RegisterCreatedObjectUndo(atomicNarrativeObjectGO, "Created Atomic Narrative Object");

			// Get a reference to the script for the Group.
			AtomicNarrativeObject atomicNarrativeObject = atomicNarrativeObjectGO.GetComponent<AtomicNarrativeObject>();

			// Set the references to essential components.
			atomicNarrativeObject.OutputSelectionDecisionPoint = atomicNarrativeObject.GetComponent<OutputSelectionDecisionPoint>();
			atomicNarrativeObject.VariableStore = atomicNarrativeObject.GetComponent<VariableStore>();

			return atomicNarrativeObject;
		}

		[MenuItem("GameObject/Cutting Room/Create/Atomic Narrative Object", false, menuItemPriority)]
		public static AtomicNarrativeObject CreateAtomicNarrativeObject()
		{
			// Create the game object.
			GameObject atomicNarrativeObjectGO = InstantiateAtomicNarrativeObject().gameObject;

			// Parent to the correct point in the hierarchy.
			AttachToEditorSelection(atomicNarrativeObjectGO);

			return atomicNarrativeObjectGO.GetComponent<AtomicNarrativeObject>();
		}

		public static GroupNarrativeObject InstantiateGroupNarrativeObject()
		{
			// Create the game object.
			GameObject groupNarrativeObjectGO = new GameObject("GroupNarrativeObject", typeof(GroupNarrativeObject));

			Undo.RegisterCreatedObjectUndo(groupNarrativeObjectGO, "Created Group Narrative Object");

			// Get a reference to the script for the Group.
			GroupNarrativeObject groupNarrativeObject = groupNarrativeObjectGO.GetComponent<GroupNarrativeObject>();

			// Set the references to essential components.
			groupNarrativeObject.OutputSelectionDecisionPoint = groupNarrativeObject.GetComponent<OutputSelectionDecisionPoint>();
			groupNarrativeObject.GroupSelectionDecisionPoint = groupNarrativeObject.GetComponent<GroupSelectionDecisionPoint>();
			groupNarrativeObject.VariableStore = groupNarrativeObject.GetComponent<VariableStore>();

			return groupNarrativeObject;
		}

		[MenuItem("GameObject/Cutting Room/Create/Group Narrative Object", false, menuItemPriority)]
		public static GroupNarrativeObject CreateGroupNarrativeObject()
		{
			// Create the game object.
			GameObject groupNarrativeObjectGO = InstantiateGroupNarrativeObject().gameObject;

			// Parent to the correct point in the hierarchy.
			AttachToEditorSelection(groupNarrativeObjectGO);

			return groupNarrativeObjectGO.GetComponent<GroupNarrativeObject>();
		}

		public static LayerNarrativeObject InstantiateLayerNarrativeObject()
		{
			GameObject layerNarrativeObjectGO = new GameObject("LayerNarrativeObject", typeof(LayerNarrativeObject));

			Undo.RegisterCreatedObjectUndo(layerNarrativeObjectGO, "Created Layer Narrative Object");

			LayerNarrativeObject layerNarrativeObject = layerNarrativeObjectGO.GetComponent<LayerNarrativeObject>();

			layerNarrativeObject.OutputSelectionDecisionPoint = layerNarrativeObject.GetComponent<OutputSelectionDecisionPoint>();
            layerNarrativeObject.LayerSelectionDecisionPoint = layerNarrativeObject.GetComponent<LayerSelectionDecisionPoint>();
            layerNarrativeObject.VariableStore = layerNarrativeObject.GetComponent<VariableStore>();

			return layerNarrativeObject;
		}

		[MenuItem("GameObject/Cutting Room/Create/Layer Narrative Object", false, menuItemPriority)]
		public static LayerNarrativeObject CreateLayerNarrativeObject()
		{
			GameObject layerNarrativeObjectGO = InstantiateLayerNarrativeObject().gameObject;

			AttachToEditorSelection(layerNarrativeObjectGO);

            return layerNarrativeObjectGO.GetComponent<LayerNarrativeObject>();
        }

		[MenuItem("GameObject/Cutting Room/Create/Graph Narrative Object", false, menuItemPriority)]
		public static GraphNarrativeObject CreateGraphNarrativeObject()
		{
			GameObject graphNarrativeObjectGO = InstantiateGraphNarrativeObject().gameObject;

			AttachToEditorSelection(graphNarrativeObjectGO);

			return graphNarrativeObjectGO.GetComponent<GraphNarrativeObject>();
		}

		public static GraphNarrativeObject InstantiateGraphNarrativeObject()
		{
			GameObject graphNarrativeObjectGO = new GameObject("GraphNarrativeObject", typeof(GraphNarrativeObject));

			Undo.RegisterCreatedObjectUndo(graphNarrativeObjectGO, "Create Graph Narrative Object");

			GraphNarrativeObject graphNarrativeObject = graphNarrativeObjectGO.GetComponent<GraphNarrativeObject>();

			graphNarrativeObject.OutputSelectionDecisionPoint = graphNarrativeObject.GetComponent<OutputSelectionDecisionPoint>();
			graphNarrativeObject.VariableStore = graphNarrativeObject.GetComponent<VariableStore>();

			return graphNarrativeObject;
		}

		private static void AttachToEditorSelection(GameObject gameObject)
		{
			gameObject.transform.parent = Selection.activeTransform;

			// Ping the object to ensure it is visible in hierarchy (unfolded).
			EditorGUIUtility.PingObject(gameObject);
		}

		//[MenuItem("CONTEXT/AtomicNarrativeObject/Set Duration To Length of MediaSource")]
		//public static async void SetDurationToLengthOfMediaSource(MenuCommand command)
		//{
		//	AtomicNarrativeObject atomicNarrativeObject = (AtomicNarrativeObject)command.context;

		//	if (atomicNarrativeObject.mediaSource != null)
		//	{
		//		MediaController mediaController = atomicNarrativeObject.mediaSource.mediaControllerPrefab.GetComponent<MediaController>();

		//		float duration = 0.0f;

		//		// Try to get the duration. If the controller has not overriden the GetDuration method, task returned is null so duration is undefined. Set to 0.0f.
		//		Task<float> getDurationTask = mediaController.GetDuration(atomicNarrativeObject.mediaSource);

		//		if (getDurationTask != null)
		//		{
		//			duration = await getDurationTask;
		//		}
		//		else
		//		{
		//			Debug.Log("Controller has provided an implementation of the GetDuration method. Duration is defaulting to 0.0f.");
		//		}

		//		atomicNarrativeObject.duration = duration;

		//		EditorUtility.SetDirty(atomicNarrativeObject);
		//	}
		//	else
		//	{
		//		atomicNarrativeObject.duration = 0.0f;

		//		EditorUtility.SetDirty(atomicNarrativeObject);
		//	}
		//}
	}
}