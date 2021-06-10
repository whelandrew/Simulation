using UnityEngine;
using UnityEditor;
using System.Collections;

namespace UnfinityGames.U2DEX
{
	/// <summary>
	/// The inspector editor for the grid component.
	/// </summary>
	[CustomEditor(typeof(u2dexGrid))]
	public class u2dexGridInspector : Editor
	{
		private static u2dexGrid currentGrid;

		//static GUIStyle inspectorLabelStyle;
		private static GUIStyle nonInspectorLabelStyle;

		private static bool initialized = false;

		public void Init()
		{
			currentGrid = (u2dexGrid)target;

			//inspectorLabelStyle = new GUIStyle(GUI.skin.label);
			nonInspectorLabelStyle = new GUIStyle(EditorStyles.label);

			nonInspectorLabelStyle.margin = new RectOffset(0, -139, nonInspectorLabelStyle.margin.top,
														nonInspectorLabelStyle.margin.bottom);
			nonInspectorLabelStyle.padding = new RectOffset(0, -139, nonInspectorLabelStyle.padding.top,
														nonInspectorLabelStyle.padding.bottom);
			initialized = true;
		}

		public override void OnInspectorGUI()
		{
			//If we haven't initialized already, manually call our Init method 
			//(as long as we're in the GUI's Layout event, otherwise currentGrid may not match between Layout and Repaint)
			if (!initialized && Event.current.type == EventType.Layout)
				Init();

			DrawGUI(true);
		}

		/// <summary>
		/// Handles all of the drawing for this Inspector.  Also allows us to externally call the rendering for our
		/// preference menu.
		/// </summary>
		/// <param name="isInspector">Whether the caller is an Inspector or not.</param>
		public static void DrawGUI(bool isInspector)
		{
			//Only draw this if we've got a grid...
			if(currentGrid != null)
			{
				currentGrid.gridSize = GlobalSnappingData.GridSize;

				//If we're using PPM, adjust our gridSize accordingly.
				if(GlobalSnappingData.UsePixelsPerMeter)
				{
					currentGrid.gridSize = currentGrid.gridSize / GlobalSnappingData.PixelsPerMeter;
				}

				//Cache our last grid size, now that we're done pre-editing it.
				var lastGridSize = currentGrid.gridSize;

				using(new UnfinityGames.Common.Editor.UnfinityHorizontal())
				{
					//Cache our label.
					GUIContent label = new GUIContent("Grid Size", "The size of the individual grid cells.");

					//Grab our labelWidth, so we can force our custom label to use it.
					var labelWidth = EditorGUIUtility.labelWidth;

					//For some reason, the labels end up taking up too much space here, so we subtract some width away.
					//To make things even more fun, the extra space varies depending on if this is in the inspector or not.
					if(!isInspector)
						labelWidth -= 18;
					else
						labelWidth -= 4;

					//Render our label, forcing it to use our calculated labelWidth.
					GUILayout.Label(label, EditorStyles.label, GUILayout.ExpandWidth(false), GUILayout.Width(labelWidth));
					
					//Set wideMode to true, to force our controls onto one line.
					var currentWideMode = EditorGUIUtility.wideMode;
					EditorGUIUtility.wideMode = true;

					//Render our controls.
					//Note: we specify MinWidth here just so the controls will shrink down as much as they can if the current window gets resized.
					currentGrid.gridSize = EditorGUILayout.Vector2Field(string.Empty, currentGrid.gridSize, GUILayout.MinWidth(10));

					//Return wideMode to whatever it was previously.
					EditorGUIUtility.wideMode = currentWideMode;
				}

				//Finally, render our grid color and grid scale fields.
				currentGrid.color = EditorGUILayout.ColorField(new GUIContent("Grid Line Color",
																"The color of the grid lines."),
																currentGrid.color);

				currentGrid.gridScale = (u2dexGrid.GridScale)EditorGUILayout.EnumPopup(new GUIContent("Grid Scale",
																			"The overall scale of the grid."),
																			currentGrid.gridScale);

				//repaint the scene with the grid.  (May not be needed, it just introduces a slight delay to the grid
				//updating if we don't have it)
				SceneView.RepaintAll();

				if(GUI.changed)
				{
					currentGrid.gridSize = FixIfNaNOrZero(currentGrid.gridSize);

					//If our gridSize was changed...
					if(currentGrid.gridSize != lastGridSize)
					{
						//Find the axis that was altered, and make sure that both axis match the changed one.
						//This enforces a square grid, which, honestly, should be the only grid type, as uneven
						//grids have too many problems for the end-user.
						if(currentGrid.gridSize.x != lastGridSize.x)
							currentGrid.gridSize.y = currentGrid.gridSize.x;

						if(currentGrid.gridSize.y != lastGridSize.y)
							currentGrid.gridSize.x = currentGrid.gridSize.y;

					}
				}

				//If we're rendering a grid inspector, draw our warning here, if needed.
				if(isInspector)
					DrawSmallGridSizeWarning();


				//Set our global grid size to our current grid's size, now that we're done.
				if(!GlobalSnappingData.UsePixelsPerMeter)
				{
					GlobalSnappingData.GridSize = currentGrid.gridSize;
				}
				else
				{
					//Return this value back to normal, now that we're done with it.
					GlobalSnappingData.GridSize = currentGrid.gridSize * GlobalSnappingData.PixelsPerMeter;
				}
			}
			else //otherwise, display an error
			{
				EditorGUILayout.HelpBox("Cannot edit Grid Size and Color, as we couldn't find a suitable U2DEX Grid to edit.\n\n" +
										"Try selecting the object in the Hierarchy tab that has the U2DEX Grid you want to edit, and then come back.",
										MessageType.Error);

				//Set our initialized flag to false, so we can try to grab a grid the next time through.
				initialized = false;
			}
		}

		/// <summary>
		/// Handles rendering the Small Grid Size Warning when the current grid size is less than 1.
		/// </summary>
		public static void DrawSmallGridSizeWarning()
		{
			//If our currentGrid isn't null...
			if(currentGrid != null)
			{
				//If our current grid size is very small (below 1) we need to display a warning box.
				if(currentGrid.gridSize.x < 1f || currentGrid.gridSize.y < 1f)
				{
					EditorGUILayout.HelpBox("WARNING!  The current grid size is less than 1, which is not recommended.\nContinue at your own risk!\n\n" +
										"The Unity Editor has been known to become unresponsive at this size on slower machines due to the large " +
										"number of lines being rendered.\n\nBecause of this you will only be able to increase the current " +
										"grid scale to 'Large'.  At grid sizes less than 1 any values higher than 'Large' will be clamped " +
										"to 'Large' in order to try and keep the Editor responsive.",
										MessageType.Warning);

					//If the current Grid Scale is greater than Large, we need to clamp it back down to Large.
					if(currentGrid.gridScale > u2dexGrid.GridScale.Large)
						currentGrid.gridScale = u2dexGrid.GridScale.Large;
				}
			}
		}

		/// <summary>
		/// Checks if a Vector2 is NaN (Not a Number) or zero, and fixes it accordingly.
		/// This also ensures that the axis of the Vector2 aren't less than 1.
		/// </summary>
		/// <param name="v"></param>
		/// <returns></returns>
		private static Vector2 FixIfNaNOrZero(Vector2 v)
		{
			if (float.IsNaN(v.x))
			{
				v.x = 0.1f; //grid can't be 0, since we'll by dividing by it later.
			}
			if (float.IsNaN(v.y))
			{
				v.y = 0.1f; //grid can't be 0, since we'll by dividing by it later.
			}

			if (v.x < 0.1f)
			{
				v.x = 0.1f;
			}
			if (v.y < 0.1f)
			{
				v.y = 0.1f;
			}

			return v;
		}
	}
}
