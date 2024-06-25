﻿namespace AtomUI.Controls;

/// <summary>
/// Defines the placement for tooltip and popover.
/// </summary>
public enum PlacementType
{
   /// <summary>
   /// The popup is placed at the pointer position.
   /// </summary>
   Pointer,
   
   /// <summary>
   /// Preferred location is below the target element.
   /// </summary>
   Bottom,
   
   /// <summary>
   /// Preferred location is to the right of the target element.
   /// </summary>
   Right,
   
   /// <summary>
   /// Preferred location is to the left of the target element.
   /// </summary>
   Left,
   
   /// <summary>
   /// Preferred location is above the target element.
   /// </summary>
   Top,
   
   /// <summary>
   /// Preferred location is above the target element, with the left edge of the popup
   /// aligned with the left edge of the target element.
   /// </summary>
   TopEdgeAlignedLeft,
   
   /// <summary>
   /// Preferred location is above the target element, with the right edge of popup aligned with right edge of the target element.
   /// </summary>
   TopEdgeAlignedRight,
   
   /// <summary>
   /// Preferred location is below the target element, with the left edge of popup aligned with left edge of the target element.
   /// </summary>
   BottomEdgeAlignedLeft,

   /// <summary>
   /// Preferred location is below the target element, with the right edge of popup aligned with right edge of the target element.
   /// </summary>
   BottomEdgeAlignedRight,

   /// <summary>
   /// Preferred location is to the left of the target element, with the top edge of popup aligned with top edge of the target element.
   /// </summary>
   LeftEdgeAlignedTop,

   /// <summary>
   /// Preferred location is to the left of the target element, with the bottom edge of popup aligned with bottom edge of the target element.
   /// </summary>
   LeftEdgeAlignedBottom,

   /// <summary>
   /// Preferred location is to the right of the target element, with the top edge of popup aligned with top edge of the target element.
   /// </summary>
   RightEdgeAlignedTop,
   
   /// <summary>
   /// Preferred location is to the right of the target element, with the bottom edge of popup aligned with bottom edge of the target element.
   /// </summary>
   RightEdgeAlignedBottom
}