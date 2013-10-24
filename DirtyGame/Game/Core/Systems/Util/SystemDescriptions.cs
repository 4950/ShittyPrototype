﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DirtyGame.game.Core.Components;
using DirtyGame.game.Core.Components.Render;
using EntityFramework;

namespace DirtyGame.game.Core.Systems.Util
{
    public class SystemDescriptions
    {
        public static SystemParams SpriteRenderSystem =
            new SystemParams(Aspect.CreateAspectFor(new List<Type> {typeof (Sprite), typeof (Spatial)}), 1);

        public static SystemParams SpawnerSystem =
            new SystemParams(Aspect.CreateAspectFor(new List<Type> { typeof (Spatial), typeof(SpawnerComponent) }), 1);

        public static SystemParams MonsterSystem =
            new SystemParams(Aspect.CreateAspectFor(new List<Type> { typeof(MonsterComponent), typeof(TimeComponent), typeof(Spatial), typeof(Sprite) }), 1);

        //public static SystemParams AISystem=
        //    new SystemParams(Aspect.CreateAspectFor(new List<Type> { typeof(AIComponent) }), 1);
    }
}
