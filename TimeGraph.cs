using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TimeGraph : MonoBehaviour 
{
	private const float		  LABEL_WIDTH	=	30.0f;
	private const float		  LABEL_HEIGHT  =   20.0f;
	private const float		  LABEL_OFFSET  =   10.0f;	// Since text isn't center aligned, we will force it to be by shifting it up by a constant value

	public bool				  m_enabled	= true;		// Draw graph?

	public Vector2 			  m_position;			// Graph origin in pixels (top-left = 0,0)
	public Vector2 			  m_size;				// Graph visual size in pixels (w,h)
	public Vector2 			  m_verticalRange;		// Value range for y component
	public int			 	  m_horizontalEntries;	// Total number of horizontal items, cannot be larger than m_size.x (1 pixel per column)
	
	public Color   			  m_baselineColor;		// Graph baseline color
	
	public TimeGraphDataSet[] m_dataSets;			// All data sets on graph

	private Material m_lineMaterial;
	private Material LineMaterial
	{
		get { return ( m_lineMaterial != null ? m_lineMaterial : CreateMaterial() ); }
		set { m_lineMaterial = value; }
	}
	
	public void PushData( float value )
	{
		PushData( 0, value );
	}
	
	public void PushData( int set, float value )
	{
		if ( ( set < 0 ) || ( set >= m_dataSets.Length) ) return;
		
		m_dataSets[ set ].PushData( value );
	}
	
	public int AddDataSet( TimeGraphDataSet dataSet )
	{
		TimeGraphDataSet[] newDataSets = new TimeGraphDataSet[ m_dataSets.Length + 1 ];
		m_dataSets.CopyTo( newDataSets, 0 );
		
		newDataSets[ newDataSets.Length - 1 ] = dataSet;
		m_dataSets = newDataSets;
		
		return newDataSets.Length - 1;
	}
	
	void Start()
	{
		ClampRanges();
	}
		
	void OnPostRender() 
	{
		if ( !m_enabled ) return;
		
		ClampRanges();
	
		GL.PushMatrix();
	
		LineMaterial.SetPass( 0 );
		GL.LoadPixelMatrix();		
		
		GL.Begin( GL.LINES );
		
		DrawGraphBaseLine();
		
		// Draw graphs
		for ( int i = 0; i < m_dataSets.Length; i++ )
		{
			DrawGraph( m_dataSets[ i ] );
		}
		
		GL.End();
		
		GL.PopMatrix();
	}
	
	void OnGUI()
	{
		GUI.Label( new Rect( m_position.x - LABEL_WIDTH, m_position.y - LABEL_OFFSET, LABEL_WIDTH, LABEL_HEIGHT ), m_verticalRange.y.ToString() );
		GUI.Label( new Rect( m_position.x - LABEL_WIDTH, m_position.y + m_size.y - LABEL_OFFSET, LABEL_WIDTH, LABEL_HEIGHT ), m_verticalRange.x.ToString() );
	}
	
	private void DrawGraph( TimeGraphDataSet data )
	{
		GL.Color( data.m_displayColor );
	
		float x = m_position.x + m_size.x;
		float x2;
		float y, y2;
		float step 	 = m_size.x / m_horizontalEntries;
		
		for ( int i = data.m_data.Count - 1; i > 0 && x > m_position.x; i-- )
		{
			x2 = x - step;
			y  = m_position.y + m_size.y - CalcPointY( data.m_data[ i ] );  
			y2 = m_position.y + m_size.y - CalcPointY( data.m_data[ i - 1 ] );  
			
			PushPoint( x, y );
			PushPoint( x2, y2 );
			
			x = x2;
		}
	}	
	
	private void DrawGraphBaseLine()
	{
		GL.Color( m_baselineColor );
		PushPoint( m_position.x, m_position.y );
		PushPoint( m_position.x, m_position.y + m_size.y );
		PushPoint( m_position.x, m_position.y + m_size.y );
		PushPoint( m_position.x + m_size.x, m_position.y + m_size.y );
		PushPoint( m_position.x + m_size.x, m_position.y + m_size.y );
		PushPoint( m_position.x + m_size.x, m_position.y );
		PushPoint( m_position.x + m_size.x, m_position.y );
		PushPoint( m_position.x, m_position.y );
	}
	
	private float CalcPointY( float y )
	{
		float yrange = m_verticalRange.y - m_verticalRange.x;
		return ( ( ( Mathf.Clamp( y, m_verticalRange.x, m_verticalRange.y ) - m_verticalRange.x ) / yrange ) * m_size.y );
	}
	
	private void PushPoint( float x, float y )
	{
		GL.Vertex3( x, InvertY( y ), 0.0f );
	}
	
	private void ClampRanges()
	{
		if ( m_horizontalEntries < 1 )
		{
			m_horizontalEntries = 1;
		}
		
		if ( m_horizontalEntries > Mathf.FloorToInt( m_size.x ) )
		{
			m_horizontalEntries = Mathf.FloorToInt( m_size.x );
		}
		
		for ( int i = 0; i < m_dataSets.Length; i++ )
		{
			m_dataSets[ i ].SetMaxEntries( m_horizontalEntries + 1 );
		}
	}
	
	private float InvertY( float y )
	{
		return ( Screen.height - y );
	}
	
	private Material CreateMaterial()
	{
		m_lineMaterial = new Material
		( 
			"Shader \"Lines/VertexColored\" {" +
		    "SubShader { Pass { " +
		    "    ZWrite Off Cull Off Fog { Mode Off } " +
		    "    BindChannels {" +
			"      Bind \"vertex\", vertex Bind \"color\", color }" +
		    "} } }" 
		);
		                            
		m_lineMaterial.hideFlags = HideFlags.HideAndDontSave;
		m_lineMaterial.shader.hideFlags = HideFlags.HideAndDontSave;	
		
		return m_lineMaterial;
	}
	
}
