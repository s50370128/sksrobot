using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;

namespace Building
{
    static class DrawTool
    {
        public static void DrawModel(Model model, StillDesign.PhysX.MathPrimitives.Matrix pose, Camera camera)
        {
            Matrix[] transforms = new Matrix[model.Bones.Count];

            model.CopyAbsoluteBoneTransformsTo(transforms);

            Matrix m = new Matrix(pose.M11, pose.M12, pose.M13, pose.M14,
                pose.M21, pose.M22, pose.M23, pose.M24,
                pose.M31, pose.M32, pose.M33, pose.M34,
                pose.M41, pose.M42, pose.M43, pose.M44);

            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.World = transforms[mesh.ParentBone.Index] * m;
                    effect.View = camera.View;
                    effect.Projection = camera.Projection;
                    effect.EnableDefaultLighting();
                    effect.SpecularColor = new Vector3(0.5f);
                    effect.Alpha = 1;
                }
                mesh.Draw();
            }
        }

        public static Matrix MatrixFromPointPtoQ(Vector3 P, Vector3 Q)
        {
            Vector3 zBasis = (Q - P);
            zBasis.Normalize();
            Vector3 axis = Vector3.Cross(zBasis, Vector3.UnitY);
            axis.Normalize();
            float angle = -(float)Math.Acos(Vector3.Dot(zBasis, Vector3.UnitY));
            return Matrix.CreateFromAxisAngle(axis, angle) * Matrix.CreateTranslation(P);
        }

        public static Matrix MatrixFromVector(Vector3 direction, Vector3 position)
        {
            direction.Normalize();
            Vector3 axis = Vector3.Cross(direction, Vector3.UnitY);
            axis.Normalize();
            float angle = -(float)Math.Acos(Vector3.Dot(direction, Vector3.UnitY));
            return Matrix.CreateFromAxisAngle(axis, angle) * Matrix.CreateTranslation(position);
        }

        public static float Distance(Vector3 P, Vector3 Q)
        {
            Vector3 d = P - Q;
            return d.Length();
        }
    }
}
