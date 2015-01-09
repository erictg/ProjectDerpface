using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ProjectDerpface.Framework.BEPUextensions;
using BEPUphysics;
using BEPUphysics.Entities;
using BEPUutilities;
using Vector3 = BEPUutilities.Vector3;
using Matrix = BEPUutilities.Matrix;

namespace ProjectDerpface.Framework.HumanoidObjects
{
    public class ControllableHumanoid : EntityModel
    {

        public ControllableHumanoid(Entity entity, Model model, BEPUutilities.Matrix transform, Game game)
            :base(entity, model, transform, game)
        {

        }


        public override void Update(GameTime gameTime)
        {

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {

            base.Draw(gameTime);
        }
    }
}
