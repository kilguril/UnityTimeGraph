using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class TimeGraphDataSet 
{
	public  Color			m_displayColor;		// Display color for this data set
	public  List<float>		m_data;
	
	private int				m_maxEntries;
	
	public TimeGraphDataSet()
	{
		m_data  = new List<float>();
	}
	
	public TimeGraphDataSet( Color color )
	{
		m_data 			= new List<float>();
		m_displayColor	= color;
	}
	
	public void PushData( float y )
	{
		while ( m_data.Count >= m_maxEntries )
		{
			m_data.RemoveAt( 0 );
		}
		
		m_data.Add( y );

	}
	
	public void SetMaxEntries( int maxEntries )
	{
		m_maxEntries = maxEntries;
	}
}
