using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HaloOnlineTagTool.Serialization;

namespace HaloOnlineTagTool.TagStructures
{
	[TagStructure(Class = "effg", Size = 0x10)]
	public class EffectGlobals
	{
		[TagElement]
		public List<TagBlock0> Unknown0 { get; set; }
		[TagElement]
		public int UnknownC { get; set; }

		[TagStructure(Size = 0x14)]
		public class TagBlock0
		{
			[TagElement]
			public int Unknown0 { get; set; }
			[TagElement]
			public int Unknown4 { get; set; }
			[TagElement]
			public List<TagBlock1> Unknown8 { get; set; }

			[TagStructure(Size = 0x10)]
			public class TagBlock1
			{
				[TagElement]
				public int Unknown0 { get; set; }
				[TagElement]
				public int Unknown4 { get; set; }
				[TagElement]
				public int Unknown8 { get; set; }
				[TagElement]
				public int UnknownC { get; set; }
			}
		}
	}
}
