-------------------------------------------------------------------------
U2DEX: Unity 2D Editor Extensions
Copyright © 2013-2019 Unfinity Games

Last updated September 6th, 2019
-------------------------------------------------------------------------

Welcome to U2DEX, and thank you for purchasing!  
This readme is split into 5 sections, Installation, Updating, Settings, Help, and the Changelog.

-------------------------------------------------------------------------

1. INSTALLATION FROM UNITY ASSET STORE

Simply download and import the package through Unity and everything should be good-to-go.  
U2DEX will automatically detect 2D Toolkit, Orthello or Unity 4.3 Sprites if you have one (or all!) of them
installed, so there is no additional required setup work besides the initial import.

If everything imported correctly, you should be ready to use U2DEX!

1a. SPECIAL NOTE FOR NGUI USERS

NGUI also overrides the default Transform Inspector and, because of this, is not compatible with U2DEX out-of-the-box.
To enable compatibility, please extract the NGUISupport.unitypackage found in the U2DEX folder in your project.
After doing so, a new option will be present in the Edit->Preferences...->U2DEX menu that will allow you to use the
NGUI Transform Inspector in place of the U2DEX Transform Inspector, if you wish to do so.

-------------------------------------------------------------------------

2. UPDATING FROM AN OLDER VERSION

There are some breaking changes from v1.40 to any newer version.
If you're upgrading, there are some steps you need to follow.

(Optional) Backup your project.  This isn't required, but it's highly recommended due to how Unity imports assets.
A. Delete the Unfinity Games/Shared and Unfinity Games/U2DEX folders.
B. Download and import the new version from the Unity Asset store (using the instructions above, if needed).
C. That's it!  You should be good to go!

There are some breaking changes from v1.10 Beta to any newer version.  
If you're upgrading, there are some steps you need to follow.

(Optional) Backup your project.  This isn't required, but it's highly recommended due to how Unity imports assets.
A. Delete the U2DEX folder.  You will unfortunately lose any preferences and settings, but from here on out
   settings are not stored in a file.  (Sorry about that!)
B. Download and import the new version from the Unity Asset store (using the instructions above, if needed).
C. Replace any Grid components you had with the new u2dexGrid component (found in the same place: Component->U2DEX->Camera->Grid).
D. Configure your settings (if needed) under the new menu found at Edit->Preferences...->U2DEX.
E. That's it!  You should be good to go!

-------------------------------------------------------------------------

3. SETTINGS

The U2DEX Settings are located under Edit->Preferences...->U2DEX.  Additionally, there are clickable menu
buttons for our forums (great for getting help!), an "About" popup that contains some useful info,
and an updater utility (to see if a new version is available, and to read its changelog if there's an update).
Additionally, the Grid component can be found under Component->U2DEX->Camera->U2DEX Grid.

There are many options that you can configure, and all of them should be pretty self-explanatory.
The only exception is "Applicable Classes".  If you're confused about their usage,
there's a built-in help file that you can access by clicking the "?" button on the 'Applicable Classes' toolbar.

-------------------------------------------------------------------------

4. HELP

Help is available either via email, or by the use of the Unfinity Games Forums.
When seeking help, we would prefer the forums as any questions posted there will help others with the 
same questions, but ultimately, if you need help either avenue is absolutely fine.

Forums: http://www.unfinitygames.com/interact
Email: http://www.unfinitygames.com/contact-us/

-------------------------------------------------------------------------

5. CHANGELOG

September 2019: v1.50
- Resolved issues with Unity 2019.
- Compressed embedded resources, resulting in a smaller overall package size.
- Fixed an issue where Texture2D.LoadImage could load our embedded editor resources with incorrect RGB values.
- Added better support for the new Editor themes coming in Unity 2019.3.
- Updated some styling for U2DEX's Preferences so they render properly in the new Preferences Window (2018.3 and above).
- Updated the Grid Size controls to render more consistently between the Inspector and Preferences.
- Snapping options are now saved immediately, once a change is detected.
- Improved how the U2DEX Grid Gizmo renders in the Scene View.
- Fixed Reflection failing to find the new Preferences Window class introduced in Unity 2018.3.
- Removed official support for Orthello, as the product is no longer maintained.
- Removed the height restriction on the Applicable Classes list.  It'll now properly use any available vertical space.
- Updated the Shared code to the latest version.

November 2017: v1.48 Release
- Fixed a case where Unity could load some textures for the Unfinity Games Updater incorrectly, resulting in color-shifted images.
- Updated the Shared code to the latest version.

March 2017: v1.47 Release
- Fixed a case where Unity could load our Editor textures incorrectly, resulting in color-shifted images.
- Updated the Shared code to the latest version.

December 2016: v1.46 Release
- Fixed a critical Reflection error in newer versions of Unity.
- Altered the Preference saving so it detects and saves changes immediately, and more reliably.
- Changed many of the internal Reflection calls to use the newer, more reliable methods used in uTemplate.
- Fixed a case where the detection of custom Applicable Classes could fail.

November 2016: v1.45 Release
- Updated the Shared code to be more in-line with our new plugin, uTemplate.
- Removed the forced fixed-width of the Preferences menu.  It'll now do its best to scale to the width of the window.
- Hooked U2DEX up to the new Unfinity Games Updater.
- U2DEX has been moved to a DLL.  This is not only easier to import (less chances for errors, or duplicated scripts), but it makes it easier to move around within your project.
- Editor Default Resources has been removed.  All of the resources needed by U2DEX are now compiled in the DLL, to help keep your project tidy.
- Fixed a warning related to LookLikeControls.
- Improved the speed of various internal method calls.
- A few other small fixes/adjustments.

January 2016: v1.40 Release
- Fixed a bug that allowed snapping to function during play-mode.
- Added an option to disable entries in the Applicable Classes list, on a per-class basis.
- Fixed an issue where objects without snapping could have their positions snapped, if the previously-selected object had snapping enabled.
- Updated the Applicable Classes help window (to reflect the new per-class toggle).
- A few other small fixes/adjustments.

August 2015: v1.33 Release
- Added support for grid sizes lower than 1.  Note that this is still not recommended, but the option is now available.
- Clamped the grid size's low end to 0.1, rather than 1.
- Added a warning help box when grid sizes are below 1.
- Clamped the grid scale to 'Large' or below when using a grid size less than 1, in an effort to keep the editor responsive when rendering many grid lines.
- Cleaned up a bit.

April 2015: v1.32 Release
- Fixed a rare but critical bug regarding certain Namespace names and Applicable Classes.

April 2015: v1.31 Release
- Changed the component menu structure to better align with our new plugin, uNote.  The new path is Component->Unfinity Games->U2DEX.
- Fixed a couple of possible sprite lighting bugs caused by changes in Unity 5's lighting system.

March 2015: v1.30 Release
- Changed one GetComponent call for full compatibility for Unity 5.  No API updater needed.
- Removed support for Unity versions older than Unity 4.3.  U2DEX now supports Unity 4.3 through Unity 5.
- Changed U2DEX's namespace from u2dex to UnfinityGames.U2DEX.
- Fixed multiple bugs related to the u2dexGrid and perspective scene cameras.
- Added new Grid Scale options to u2dexGrid.  You can now select from multiple sizes, in case larger sizes slow down your editor too much.
- Renamed u2dexGrid's component menu entry from 'Grid' to 'U2DEX Grid'.
- Improved how Grid Size's X and Y values work in the u2dexGridInspector.
- Fixed a bug where u2dexGrid wasn't resetting the Gizmo.color to whatever value it was before rendering the grid.
- Restructured the U2DEX directory.  U2DEX is now an Unfinity Games folder, as we now have a Shared folder for common utilities between our plugins.
- Moved our images into Editor Default Resources.  This means that our images will never be included in your builds.
- Drastically improved support for both the Light and Dark editor skins.  We now fully support both.
- Consolidated our Editor Events into one file, as well as moving them to our new UnfinityGames.Common.EditorEvents namespace.
- Added some missing tooltips to the u2dexGridInspector.
- Changed the Updater and Changelog Viewer to be asset-agnostic, as well as renaming them.
- u2dexEditorPrefs is now UnfinityEditorPrefs, and now resides in the Shared folder.
- Fixed some Transform Inspector inconsistencies regarding text color and foldout icon sizes.
- Fixed an issue with the Help dialog's 'Close' button not actually closing the dialog.
- Removed u2dexMenu.cs, as it was no longer needed.
- Did some clean up in all of our classes and added some extra comments where needed.
- Updated our NGUISupport.unitypackage.
- Added a warning in U2DEX's preferences that will show up if NGUI is detected, but the user hasn't imported our support package.
- Moved FixIfNaN into the base TransformInspector2D class, as it didn't need to be in every child class.
- Clamped the low-end of u2dexGrid's size to 1, as values below 1 unit behaved erratically in Unity.
- Improved support for moving U2DEX's folder to another sub-folder in the project.  U2DEX no longer assumes it's in the root directory. 
- Resolved an Editor GUI Repaint issue when going into or leaving play mode.
- The UnfinityUpdater will now close when the user has chosen to view a changelog on the internet.
- Updated all of U2DEX's Asset Store/Promotional images.

August 2014: v1.20 Release
- Renamed the Grid component to u2dexGrid for better name continuity.  See the Updating section in the Readme to find out how this affects you.
- Renamed the GridInspector to u2dexGridInspector for better name continuity.
- Moved everything U2DEX-related to the Unity Preferences area, under U2DEX.
- Added additional settings to the new Preference menu.
- Settings and Preferences are now saved to EditorPrefs, not a settings file.  The old settings file can be deleted.
- Preferences will now follow you between projects, so you don't have to configure settings for every project.
- Separated out some Editor UI stuff so it's not tied down to an EditorWindow.  
- Fixed some typos in variable declarations.
- Rewrote the grid component so it draws a grid over the entire screen, even while zoomed out.
- Added Pixel Per Meter support to the Grid Component.
- Added a fix for an incompatibility with NGUI (unpack the NGUI Support .unitypackage).
- Added an option to use the NGUI Transform Inspector instead of the U2DEX Transform Inspector.
- Reworked the U2DEX Grid Inspector a bit.

December 2013: v1.10 Beta
- Added full support for Unity 3.x versions, along with the already-supported 4.x versions.
- Added full support for Unity's 2D Sprites! (Requires Unity 4.3)
- Added support for play-mode editing.  Everything now works if you pause and edit while a game is running.
- Fixed a rare error that could occur when using MonoDevelop on versions greater than 4.2.1.
- Bulletproofed some code that could, in theory, throw some non-descriptive errors.
- Added a utility to check if new updates are available.
- Added a utility to view the changelog for the newest update, if there is one.
- Incremented version number of the save file format (format changed slightly, all old save files will be upgraded...)
- Re-labeled some controls so they're easier to understand at a glance.
- Changed some of the Inspectors and Editor Windows to better match Unity 4.3 (Only applies if using 4.3 or greater)
- Removed the need for Orthello sprites to have a valid material before the Transform Inspector would apply.
- Added a note on Orthello sprite Inspectors if the material is null.

October 2013: v1.00 Beta
- Initial Release

-------------------------------------------------------------------------