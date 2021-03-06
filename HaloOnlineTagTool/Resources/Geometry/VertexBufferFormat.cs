﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HaloOnlineTagTool.Resources.Geometry
{
	/// <summary>
	/// Vertex buffer formats.
	/// </summary>
	/// <remarks>
	/// This enum is total garbage and is only used to calculate vertex sizes.
	/// All that matters is that the corresponding size matches the vertex buffer.
	/// </remarks>
	public enum VertexBufferFormat : short
	{
		Invalid,      // Invalid
		World,        // Size = 0x38
		Rigid,        // Size = 0x38
		Skinned,      // Size = 0x40
		Unknown4,     // Size = 0x8
		Unknown5,     // Size = 0x4
		Unknown6,     // Size = 0x14
		Unknown7,     // Size = 0x14
		Unused8,      // Invalid
		AmbientPrt,   // Size = 0x4
		LinearPrt,    // Size = 0x4
		QuadraticPrt, // Size = 0x24
		UnknownC,     // Size = 0x14
		UnknownD,     // Size = 0x10
		UnknownE,     // Size = 0xC
		UnknownF,     // Size = 0x18
		Unused10,     // Invalid
		Unused11,     // Invalid
		Unused12,     // Invalid
		Unused13,     // Invalid
		Unknown14,    // Size = 0x8
		Unknown15,    // Size = 0x4
		Unknown16,    // Size = 0x4
		Unknown17,    // Size = 0x4
		Unknown18,    // Size = 0x20
		Unknown19,    // Size = 0x20
		Unknown1A,    // Size = 0xC
		Unknown1B,    // Size = 0x24
		Unknown1C,    // Size = 0x80
		Unused1D,     // Invalid
	}
}
