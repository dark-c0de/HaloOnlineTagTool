﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HaloOnlineTagTool.TagStructures;

namespace HaloOnlineTagTool.Resources.Geometry
{
	/// <summary>
	/// Provides functions for reading mesh data.
	/// </summary>
	public class MeshReader
	{
		private const int StreamCount = 5;
		private const int IndexBufferCount = 2;

		/// <summary>
		/// Initializes a new instance of the <see cref="MeshReader"/> class.
		/// </summary>
		/// <param name="mesh">The mesh.</param>
		/// <param name="definition">The mesh's definition data.</param>
		public MeshReader(Mesh mesh, RenderGeometryResourceDefinition definition)
		{
			Mesh = mesh;
			Definition = definition;
			VertexStreams = new VertexBufferDefinition[StreamCount];
			IndexBuffers = new IndexBufferDefinition[IndexBufferCount];
			BindVertexStreams();
			BindIndexBuffers();
		}

		/// <summary>
		/// Gets the mesh.
		/// </summary>
		public Mesh Mesh { get; private set; }

		/// <summary>
		/// Gets the mesh's definition data.
		/// </summary>
		public RenderGeometryResourceDefinition Definition { get; private set; }

		/// <summary>
		/// Gets the vertex streams for the mesh. Note that elements can be <c>null</c>.
		/// </summary>
		public VertexBufferDefinition[] VertexStreams { get; private set; }

		/// <summary>
		/// Gets the index streams for the mesh. Note that elements can be <c>null</c>.
		/// </summary>
		public IndexBufferDefinition[] IndexBuffers { get; private set; }

		/// <summary>
		/// Opens a vertex reader on one of the mesh's vertex buffers.
		/// </summary>
		/// <param name="definition">The vertex buffer definition.</param>
		/// <param name="baseStream">The stream open on the mesh's resource data to use as a base stream.</param>
		/// <returns>The vertex reader if successful, or <c>null</c> otherwise.</returns>
		public VertexReader OpenVertexReader(VertexBufferDefinition definition, Stream baseStream)
		{
			if (definition.Data.Address.Type != ResourceAddressType.Resource)
				return null; // Don't bother supporting non-resource addresses
			baseStream.Position = definition.Data.Address.Offset;
			return new VertexReader(new BinaryReader(baseStream));
		}

		/// <summary>
		/// Opens a vertex reader on one of the mesh's vertex streams.
		/// </summary>
		/// <param name="streamIndex">Index of the vertex stream to open.</param>
		/// <param name="baseStream">The stream open on the mesh's resource data to use as a base stream.</param>
		/// <returns>The vertex reader if successful, or <c>null</c> otherwise.</returns>
		public VertexReader OpenVertexReader(int streamIndex, Stream baseStream)
		{
			if (streamIndex < 0 || streamIndex >= VertexStreams.Length)
				return null;
			var definition = VertexStreams[streamIndex];
			if (definition == null)
				return null;
			return OpenVertexReader(definition, baseStream);
		}

		/// <summary>
		/// Opens an index buffer reader on one of the mesh's index buffers.
		/// </summary>
		/// <param name="definition">The index buffer definition.</param>
		/// <param name="baseStream">The stream open on the mesh's resource data to use as a base stream.</param>
		/// <returns>The index buffer reader if successful, or <c>null</c> otherwise.</returns>
		public IndexBufferReader OpenIndexBufferReader(IndexBufferDefinition definition, Stream baseStream)
		{
			if (definition.Data.Address.Type != ResourceAddressType.Resource)
				return null; // Don't bother supporting non-resource addresses
			baseStream.Position = definition.Data.Address.Offset;
			return new IndexBufferReader(new BinaryReader(baseStream), IndexBufferFormat.UInt16);
		}

		/// <summary>
		/// Opens an index buffer reader on one of the mesh's index buffers.
		/// </summary>
		/// <param name="bufferIndex">Index of the index buffer to open.</param>
		/// <param name="baseStream">The stream open on the mesh's resource data to use as a base stream.</param>
		/// <returns>The index buffer reader if successful, or <c>null</c> otherwise.</returns>
		public IndexBufferReader OpenIndexBufferReader(int bufferIndex, Stream baseStream)
		{
			if (bufferIndex < 0 || bufferIndex >= IndexBuffers.Length)
				return null;
			var definition = IndexBuffers[bufferIndex];
			if (definition == null)
				return null;
			return OpenIndexBufferReader(definition, baseStream);
		}

		/// <summary>
		/// Binds each vertex buffer in the mesh to a stream.
		/// </summary>
		private void BindVertexStreams()
		{
			// The game actually loads buffers from specific indexes, but it's
			// dependent upon what type of mesh is being loaded. Instead, we
			// can just scan the entire array and bind everything as an
			// approximation.
			foreach (var bufferIndex in Mesh.VertexBuffers.Where(i => i < Definition.VertexBuffers.Count))
			{
				// Use the buffer format to determine the stream index
				var buffer = Definition.VertexBuffers[bufferIndex].Definition;
				int streamIndex;
				if (!VertexBufferStreams.TryGetValue(buffer.Format, out streamIndex))
					continue;
				VertexStreams[streamIndex] = buffer;
			}
		}

		/// <summary>
		/// Binds each index buffer in the mesh.
		/// </summary>
		private void BindIndexBuffers()
		{
			if (Mesh.IndexBuffers.Length < IndexBuffers.Length)
				throw new InvalidOperationException("Mesh has too few index buffers");
			for (var i = 0; i < IndexBuffers.Length; i++)
			{
				var bufferIndex = Mesh.IndexBuffers[i];
				if (bufferIndex < Definition.IndexBuffers.Count)
					IndexBuffers[i] = Definition.IndexBuffers[bufferIndex].Definition;
			}
		}

		// Maps vertex buffer formats to their corresponding streams.
		private static readonly Dictionary<VertexBufferFormat, int> VertexBufferStreams = new Dictionary<VertexBufferFormat, int>
		{
			{ VertexBufferFormat.Invalid, 0 },
			{ VertexBufferFormat.World, 0 },
			{ VertexBufferFormat.Rigid, 0 },
			{ VertexBufferFormat.Skinned, 0 },
			{ VertexBufferFormat.Unknown4, 4 },
			{ VertexBufferFormat.Unknown5, 4 },
			{ VertexBufferFormat.Unknown6, 4 },
			{ VertexBufferFormat.Unknown7, 0 },
			{ VertexBufferFormat.Unused8, 2 },
			{ VertexBufferFormat.AmbientPrt, 2 },
			{ VertexBufferFormat.LinearPrt, 2 },
			{ VertexBufferFormat.QuadraticPrt, 2 },
			{ VertexBufferFormat.UnknownC, 0 },
			{ VertexBufferFormat.UnknownD, 0 },
			{ VertexBufferFormat.UnknownE, 4 },
			{ VertexBufferFormat.UnknownF, 0 },
			{ VertexBufferFormat.Unused10, 1 },
			{ VertexBufferFormat.Unused11, 2 },
			{ VertexBufferFormat.Unused12, 1 },
			{ VertexBufferFormat.Unused13, 1 },
			{ VertexBufferFormat.Unknown14, 1 },
			{ VertexBufferFormat.Unknown15, 2 },
			{ VertexBufferFormat.Unknown16, 2 },
			{ VertexBufferFormat.Unknown17, 2 },
			{ VertexBufferFormat.Unknown18, 0 },
			{ VertexBufferFormat.Unknown19, 0 },
			{ VertexBufferFormat.Unknown1A, 2 },
			{ VertexBufferFormat.Unknown1B, 3 },
			{ VertexBufferFormat.Unknown1C, 0 },
			{ VertexBufferFormat.Unused1D, 1 },
		};
	}
}
