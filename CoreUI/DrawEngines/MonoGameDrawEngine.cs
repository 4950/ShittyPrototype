﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace CoreUI.DrawEngines
{
    public class MonoGameTexture : IUITexture
    {
        public Texture2D texture;
        public MonoGameTexture(Texture2D tex)
        {
            texture = tex;
        }
        public void Delete()
        {
            texture.Dispose();
        }
    }
    public class MonoGameColor : IUIColor
    {
        public Color color;
        public MonoGameColor(Color c)
        {
            color = c;
        }
    }
    public class MonoGameRenderSurface : IUIRenderSurface
    {
        public RenderTarget2D target;
        public MonoGameRenderSurface(RenderTarget2D tar)
        {
            target = tar;
        }
        public void resize(int width, int height)
        {
            RenderTarget2D n = new RenderTarget2D(target.GraphicsDevice, width, height, false, SurfaceFormat.Color, DepthFormat.None);
            target.Dispose();
            target = n;
        }
    }

    public class MonoGameDrawEngine : IDrawEngine
    {
        SpriteBatch batch;
        GraphicsDevice device;
        Matrix? transform = null;
        ContentManager content;
        IUIRenderSurface cur;
        BasicEffect eff;
        SpriteFont defaultFont;

        public MonoGameDrawEngine(GraphicsDevice dev, ContentManager cont)
        {
            content = cont;
            device = dev;

            batch = new SpriteBatch(device);

        }
        public void setDefaultFont(SpriteFont font)
        {
            defaultFont = font;
        }
        public IUIRenderSurface CreateRenderSurface(int width, int height)
        {
            RenderTarget2D tar = new RenderTarget2D(device, width, height, false, SurfaceFormat.Color, DepthFormat.None);
            return new MonoGameRenderSurface(tar);
        }
        public void BeginDraw(IUIRenderSurface ren)
        {
            cur = ren;
            device.SetRenderTarget((ren as MonoGameRenderSurface).target);
            device.Clear(Color.Transparent);
            BeginDraw();
        }
        public void setSize(int width, int height)
        {
            eff = new BasicEffect(device);
            eff.VertexColorEnabled = true;
            eff.Projection = Matrix.CreateOrthographicOffCenter
            (0, (float)device.Viewport.Width,     // left, right
            (float)device.Viewport.Width, 0,    // bottom, top
            1, 1000);
        }
        public IUIColor CreateColor(float r, float g, float b, float a)
        {
            Color c = new Color(r, g, b, a);
            return new MonoGameColor(c);
        }
        public IUIColor CreateColor(float r, float g, float b)
        {
            Color c = new Color(r, g, b);
            return new MonoGameColor(c);
        }
        public IUIColor CreateColor(int argb)
        {
            return CreateColor(System.Drawing.Color.FromArgb(argb));
        }
        public IUIColor CreateColor(System.Drawing.Color color)
        {
            Color c = new Color(color.R / 255f, color.G / 255f, color.B / 255f, color.A / 255f);
            return new MonoGameColor(c);
        }
        public void Draw_RS(IUIRenderSurface tex, int left, int top, int right, int bottom)
        {
            MonoGameRenderSurface r = tex as MonoGameRenderSurface;
            batch.Draw(r.target, toRect(left, top, right, bottom), Color.White);
        }
        public IUITexture CreateTexture(byte[] data)
        {
            Texture2D tex = //new Texture2D(device, 0, 0);
            Texture2D.FromStream(device, new System.IO.MemoryStream(data));
            
            //tex.SetData(data);
            return new MonoGameTexture(tex);
        }
        public IUITexture CreateTexture(String filename)
        {
            Texture2D tex = content.Load<Texture2D>(filename);
            return new MonoGameTexture(tex);
        }
        public void setViewMatrix(Matrix? view)
        {
            transform = view;
        }
        public void BeginDraw()
        {
            if (transform != null)
            {
                
                batch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullCounterClockwise, null, (Matrix)transform);
            }
            else
            {
                batch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullCounterClockwise, null);
            }
            device.SamplerStates[0].MaxAnisotropy = 16;

        }
        public void EndDraw()
        {
            batch.End();
            if (cur != null)
            {
                device.SetRenderTarget(null);
                cur = null;
            }
        }
        private Rectangle toRect(int left, int top, int right, int bottom)
        {
            return new Rectangle(left, top, right - left, bottom - top);
        }
        public void Draw_Texture(IUITexture tex, int left, int top, int right, int bottom)
        {
            batch.Draw((tex as MonoGameTexture).texture, toRect(left, top, right, bottom), Color.White);
        }

        public void Draw_FilledBox(int left, int top, int right, int bottom, IUIColor color)
        {
            Texture2D rect = new Texture2D(device, 2, 2, false, SurfaceFormat.Color);
            Color[] rectData = new Color[4];
            for (int i = 0; i < 4; i++)
                rectData[i] = (color as MonoGameColor).color;
            rect.SetData(rectData);
            batch.Draw(rect, toRect(left, top, right, bottom), Color.White);
        }

        public void Draw_FilledBox(int left, int top, int right, int bottom, IUIColor color1, IUIColor color2, IUIColor color3, IUIColor color4)
        {
            Texture2D rect = new Texture2D(device, 2, 2, false, SurfaceFormat.Color);
            Color[] rectData = new Color[4];
            rectData[0] = (color1 as MonoGameColor).color;
            rectData[1] = (color2 as MonoGameColor).color;
            rectData[2] = (color3 as MonoGameColor).color;
            rectData[3] = (color4 as MonoGameColor).color;
            rect.SetData(rectData);

            device.SamplerStates[0].Filter = TextureFilter.Anisotropic;
            batch.Draw(rect, toRect(left, top, right, bottom), Color.White);
            device.SamplerStates[0].Filter = TextureFilter.Point;

        }

        public void Draw_Box(int left, int top, int right, int bottom, IUIColor color)
        {
            Draw_Line(left, top, right, top, color);
            Draw_Line(right, top, right, bottom, color);
            Draw_Line(left, bottom, right, bottom, color);
            Draw_Line(left, top, left, bottom, color);
        }

        public void Draw_Line(int left, int top, int right, int bottom, IUIColor color)
        {
            Draw_Line(left, top, right, bottom, color, color);
        }

        public void Draw_Line(int left, int top, int right, int bottom, IUIColor color1, IUIColor color2)
        {

            Texture2D line = new Texture2D(device, 2, 1, false, SurfaceFormat.Color);
            Color[] lineData = new Color[2];
            lineData[0] = (color1 as MonoGameColor).color;
            lineData[1] = (color2 as MonoGameColor).color;
            line.SetData(lineData);

            int width = 1;
            Vector2 begin = new Vector2(left, top);
            Vector2 end = new Vector2(right, bottom);

            //test
            /*eff.CurrentTechnique.Passes[0].Apply();
            VertexPositionColor[] va = new VertexPositionColor[2];
            va[0].Position = new Vector3(left, top, 0);
            va[0].Color = (color1 as MonoGameColor).color;
            va[1].Position = new Vector3(right, bottom, 0);
            va[1].Color = (color2 as MonoGameColor).color;
            device.DrawUserPrimitives<VertexPositionColor>(PrimitiveType.LineList, va, 0, 2);*/

            Rectangle r = new Rectangle((int)begin.X, (int)begin.Y, (int)(end - begin).Length() + width, width);
            Vector2 v = Vector2.Normalize(begin - end);
            float angle = (float)Math.Acos(Vector2.Dot(v, -Vector2.UnitX));
            if (begin.Y > end.Y) angle = MathHelper.TwoPi - angle;

            device.SamplerStates[0].Filter = TextureFilter.Anisotropic;
            batch.Draw(line, r, null, Color.White, angle, Vector2.Zero, SpriteEffects.None, 0);
            device.SamplerStates[0].Filter = TextureFilter.Point;
        }

        public void Draw_Default_Text(string text, int left, int top, IUIColor color)
        {
            if (defaultFont != null && text != "")
                batch.DrawString(defaultFont, text, new Vector2(left, top), (color as MonoGameColor).color);
            //throw new NotImplementedException();
        }
        public System.Drawing.PointF getTextSize(String text, IUIFont font)
        {
            Vector2 s = defaultFont.MeasureString(text);
            return new System.Drawing.PointF(s.X, s.Y);
        }
        public void Draw_Default_Text(string text, int left, int top, IUIColor color, IUIFont font)
        {
            if (defaultFont != null && text != "")
                batch.DrawString(defaultFont, text, new Vector2(left, top), (color as MonoGameColor).color);
            //throw new NotImplementedException();
        }
    }
}