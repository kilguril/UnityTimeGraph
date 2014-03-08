UnityTimeGraph
==============

A tiny utility for Unity3D for graphing a data sets over time.

Usage
-----

1. Add TimeGraph.cs and TimeGraphData.cs source files to your project.
2. Attach the TimeGraph script to your camera GameObject. 

**NOTE:** the script uses the "OnPostRender()" message to render the graph, and will not work unless attached to a camera.

Public Parameters
-----------------

These parameters are serialized in the editor, and can be set by modifying the values in the inspector or through code.

- **Enabled (m_enabled)** - *Boolean*, if set to false the graph will not render.
- **Position (m_position)** - *Vector2*, defines the graph position as an offset in pixels from the top left corner.
- **Size (m_size)** - *Vector2*, the size of the graph in pixels ( width, height ).
- **Vertical Range (m_verticalRange)** - *Vector2*, the y value range ( min, max ). Values outside this range will be clamped.
- **Horizontal Entries (m_horizontalEntries)** - *Int*, total number of times to display on the x axis. Value will be automatically clamped to the range of ( 1, m_size.x ). Can never display at once more items than the actual width of the graph ( in which case each item is represented by 1 pixel width ).
- **Baseline Color (m_baselineColor)** - *Color*, defines the color of the graph outline.


Public Methods
--------------

**TimeGraphDataSet:**
- **TimeGraphDataSet()** - Creates a new data set.
- **TimeGraphDataSet( Color color )** - Creates a new data set and assigns set color to color.
- **void PushData( float y )** - Pushes a new value into the set.
- **void SetMaxEntries( int maxEntries )** - Sets the maximal number of data entries in the set. Pushing data that would exceed this limit will remove older entires.
 
**TimeGraph:**
- **int AddDataSet( TimeGraphDataSet dataSet )** - Adds the specified data set to the graph and returns it's index.
- **void PushData( float value )** - Pushes specified data to the data set at index 0. If non existent will silently fail.
- **void PushData( int set, float value )** - Pushes specified data to the data set at index denoted by set. If set does not exists will silently fail. 


