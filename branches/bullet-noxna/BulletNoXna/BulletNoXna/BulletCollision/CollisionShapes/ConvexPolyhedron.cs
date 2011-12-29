﻿/*
 * C# / XNA  port of Bullet (c) 2011 Mark Neale <xexuxjy@hotmail.com>
 *
 * Bullet Continuous Collision Detection and Physics Library
 * Copyright (c) 2003-2008 Erwin Coumans  http://www.bulletphysics.com/
 *
 * This software is provided 'as-is', without any express or implied warranty.
 * In no event will the authors be held liable for any damages arising from
 * the use of this software.
 * 
 * Permission is granted to anyone to use this software for any purpose, 
 * including commercial applications, and to alter it and redistribute it
 * freely, subject to the following restrictions:
 * 
 * 1. The origin of this software must not be misrepresented; you must not
 *    claim that you wrote the original software. If you use this software
 *    in a product, an acknowledgment in the product documentation would be
 *    appreciated but is not required.
 * 2. Altered source versions must be plainly marked as such, and must not be
 *    misrepresented as being the original software.
 * 3. This notice may not be removed or altered from any source distribution.
 */

using System.Collections.Generic;
using System.Diagnostics;
using BulletXNA.LinearMath;
using Microsoft.Xna.Framework;

namespace BulletXNA.BulletCollision
{


public class Face
{
	public ObjectArray<int>	m_indices = new ObjectArray<int>();
	public ObjectArray<int>	m_connectedFaces = new ObjectArray<int>();
	public float[]	m_plane = new float[4];
}


public class ConvexPolyhedron
{
	public ConvexPolyhedron()
    {
    }
	public virtual void Cleanup()
    {

    }


	public void	Initialize()
    {
	Dictionary<InternalVertexPair,InternalEdge> edges = new Dictionary<InternalVertexPair,InternalEdge>();

	float TotalArea = 0.0f;
	
	m_localCenter = Vector3.Zero;
	for(int i=0;i<m_faces.Count;i++)
	{
		int numVertices = m_faces[i].m_indices.Count;
		int NbTris = numVertices;
		for(int j=0;j<NbTris;j++)
		{
			int k = (j+1)%numVertices;
			InternalVertexPair vp = new InternalVertexPair(m_faces[i].m_indices[j],m_faces[i].m_indices[k]);
			InternalEdge edptr = edges[vp];
			Vector3 edge = m_vertices[vp.m_v1]-m_vertices[vp.m_v0];
			edge.Normalize();

			bool found = false;

			for (int p=0;p<m_uniqueEdges.Count;p++)
			{
				
				if (MathUtil.IsAlmostZero(m_uniqueEdges[p]-edge) || 
					MathUtil.IsAlmostZero(m_uniqueEdges[p]+edge))
				{
					found = true;
					break;
				}
			}

			if (!found)
			{
				m_uniqueEdges.Add(edge);
			}

			if (edptr != null)
			{
				Debug.Assert(edptr.m_face0>=0);
				Debug.Assert(edptr.m_face1<0);
				edptr.m_face1 = (int)i;
			} else
			{
				InternalEdge ed = new InternalEdge();
				ed.m_face0 = i;
				edges[vp] =ed;
			}
		}
	}

	for(int i=0;i<m_faces.Count;i++)
	{
		int numVertices = m_faces[i].m_indices.Count;
		m_faces[i].m_connectedFaces.Resize(numVertices);

		for(int j=0;j<numVertices;j++)
		{
			int k = (j+1)%numVertices;
			InternalVertexPair vp = new InternalVertexPair(m_faces[i].m_indices[j],m_faces[i].m_indices[k]);
			InternalEdge edptr = edges[vp];
			Debug.Assert(edptr != null);
			Debug.Assert(edptr.m_face0>=0);
			Debug.Assert(edptr.m_face1>=0);

			int connectedFace = (edptr.m_face0==i)?edptr.m_face1:edptr.m_face0;
			m_faces[i].m_connectedFaces[j] = connectedFace;
		}
	}

	for(int i=0;i<m_faces.Count;i++)
	{
		int numVertices = m_faces[i].m_indices.Count;
		int NbTris = numVertices-2;
		
		Vector3 p0 = m_vertices[m_faces[i].m_indices[0]];
		for(int j=1;j<=NbTris;j++)
		{
			int k = (j+1)%numVertices;
			Vector3 p1 = m_vertices[m_faces[i].m_indices[j]];
			Vector3 p2 = m_vertices[m_faces[i].m_indices[k]];
			float Area = Vector3.Cross((p0 - p1),(p0 - p2)).Length() * 0.5f;
			Vector3 Center = (p0+p1+p2)/3.0f;
			m_localCenter += Area * Center;
			TotalArea += Area;
		}
	}
	m_localCenter /= TotalArea;


    }

    public void Project(ref Matrix trans, ref Vector3 dir, out float min, out float max)
    {
	    min = float.MaxValue;
	    max = float.MinValue;
	    int numVerts = m_vertices.Count;
	    for(int i=0;i<numVerts;i++)
	    {
            Vector3 pt = Vector3.Transform(m_vertices[i], trans);
            float dp = Vector3.Dot(pt, dir);
		    if(dp < min)	min = dp;
		    if(dp > max)	max = dp;
	    }
	    if(min>max)
	    {
		    float tmp = min;
		    min = max;
		    max = tmp;
	    }
    }

    public ObjectArray<Vector3> m_vertices = new ObjectArray<Vector3>();
	public ObjectArray<Face>	m_faces = new ObjectArray<Face>();
	public ObjectArray<Vector3>	 m_uniqueEdges= new ObjectArray<Vector3>();
	public Vector3 m_localCenter;
}

public class InternalVertexPair
{
	public InternalVertexPair(int v0,int v1)
	{
        m_v0 = v1>v0?v1:v0;
        m_v1 = v1>v0?v0:v1;

	}
	public int GetHash()
	{
		return m_v0+(m_v1<<16);
	}
	public bool Equals(ref InternalVertexPair other)
	{
		return m_v0==other.m_v0 && m_v1==other.m_v1;
	}
	public int m_v0;
	public int m_v1;
}

    public class InternalEdge
{
	public InternalEdge()
	{
        m_face0 = -1;
        m_face1 = -1;
	}
	public int m_face0;
	public int m_face1;
}
	

}