using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace _2D_Satisfactory.Screens.Crafting;

/// <summary>
/// A class representing an iron ingot (a rectangle composed of two triangles)
/// </summary>
public class IronIngot
{
    /// <summary>
    /// The vertices of the iron ingot
    /// </summary>
    VertexPositionColor[] vertices;

    /// <summary>
    /// The vertex indices of the iron ingot
    /// </summary>
    short[] indices;

    /// <summary>
    /// The effect to use rendering the triangle
    /// </summary>
    BasicEffect effect;

    /// <summary>
    /// The game this cube belongs to 
    /// </summary>
    Game game;

    /// <summary>
    /// Constructs the Iron Ingot
    /// </summary>
    /// <param name="game">The Game the Iron Ingot belongs to</param>
    /// <param name="bounds">The screen-space rectangle to draw the Iron Ingot within</param>
    public IronIngot(Game game, Rectangle bounds)
    {
        this.game = game;
        InitializeVertices();
        InitializeIndices();
        InitializeEffect(bounds);
    }   

    /// <summary>
    /// Draws the iron ingot
    /// </summary>
    public void Draw()
    {
        // Cache the old blend state 
        BlendState oldBlendState = game.GraphicsDevice.BlendState;

        // Enable alpha blending 
        game.GraphicsDevice.BlendState = BlendState.AlphaBlend;

        // Apply our effect
        effect.CurrentTechnique.Passes[0].Apply();

        // Render the quad
        game.GraphicsDevice.DrawUserIndexedPrimitives<VertexPositionColor>(
            PrimitiveType.TriangleList,
            vertices,   // The vertex collection
            0,          // The starting index in the vertex array
            4,          // The number of indices in the shape
            indices,    // The index collection
            0,          // The starting index in the index array
            2           // The number of triangles to draw
        );

        // Restore the old blend state 
        game.GraphicsDevice.BlendState = oldBlendState;
    }

    /// <summary>
    /// Initializes the vertices of our iron ingot
    /// </summary>
    private void InitializeVertices() {
        vertices = new VertexPositionColor[4];
        // vertex 0
        vertices[0].Position = new Vector3(0, 0, 0);
        vertices[0].Color = new Color(85, 85, 90);
        // vertex 1
        vertices[1].Position = new Vector3(0.9950f, 0, 0.1f);
        vertices[1].Color = new Color(150, 150, 155);
        // vertex 2 
        vertices[2].Position = new Vector3(-0.0402f, 0.9156f, 0.4f);
        vertices[2].Color = new Color(160, 160, 165);
        // vertex 3
        vertices[3].Position = new Vector3(0.9548f, 0.9156f, 0.5f);
        vertices[3].Color = new Color(225, 225, 230);
        // vertex 0
        vertices[0].Position = new Vector3(0, 0, 0);
        vertices[0].Color = new Color(85, 85, 90);
        // vertex 1
        vertices[1].Position = new Vector3(0.9950f, 0.1f, 0);
        vertices[1].Color = new Color(150, 150, 155);
        // vertex 2 
        vertices[2].Position = new Vector3(-0.0402f, 0.4f, -0.9156f);
        vertices[2].Color = new Color(160, 160, 165);
        // vertex 3
        vertices[3].Position = new Vector3(0.9548f, 0.5f, -0.9156f);
        vertices[3].Color = new Color(225, 225, 230);
    }

    /// <summary>
    /// Initialize the indices of our iron ingot
    /// </summary>
    private void InitializeIndices() {
        indices = new short[6];

        // Define triangle 0 
        indices[0] = 0;
        indices[1] = 2;
        indices[2] = 1;
        // define triangle 1
        indices[3] = 1;
        indices[4] = 2;
        indices[5] = 3;
    }

    /// <summary>
    /// Initializes the basic effect used to draw the iron ingot
    /// </summary>
    /// <param name="bounds">The screen-space rectangle to draw the iron ingot within</param>
    private void InitializeEffect(Rectangle bounds)
    {
        const float cameraDistance = 4f;
        const float fieldOfView = MathHelper.PiOver4;
        Viewport viewport = game.GraphicsDevice.Viewport;

        // The size of one screen pixel in world units on the z = 0 plane
        float worldUnitsPerPixel = 2 * cameraDistance * MathF.Tan(fieldOfView / 2) / viewport.Height;

        // Scale the ingot to fit within the bounds (with some padding)
        BoundingBox box = BoundingBox.CreateFromPoints(vertices.Select(v => v.Position));
        Vector3 size = box.Max - box.Min;
        float scale = MathF.Min(bounds.Width / size.X, bounds.Height / size.Y) * worldUnitsPerPixel * 0.8f;

        // The center of the bounds in world space
        Vector3 position = new Vector3(
            (bounds.Center.X - viewport.Width / 2f) * worldUnitsPerPixel,
            (viewport.Height / 2f - bounds.Center.Y) * worldUnitsPerPixel,
            0
        );

        effect = new BasicEffect(game.GraphicsDevice);
        effect.World = Matrix.CreateTranslation(-(box.Min + box.Max) / 2)   // Center the ingot on the origin
            * Matrix.CreateScale(scale)                                     // Shrink it to fit the bounds
            * Matrix.CreateTranslation(position);                           // Move it to the bounds
        effect.View = Matrix.CreateLookAt(
            new Vector3(0, 0, cameraDistance), // The camera position
            new Vector3(0, 0, 0), // The camera target,
            Vector3.Up            // The camera up vector
        );
        effect.Projection = Matrix.CreatePerspectiveFieldOfView(
            fieldOfView,                                // The field-of-view 
            viewport.AspectRatio,                       // The aspect ratio
            0.1f, // The near plane distance 
            100.0f // The far plane distance
        );
        effect.VertexColorEnabled = true;
    }
}