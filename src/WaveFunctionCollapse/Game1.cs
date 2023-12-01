using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace WaveFunctionCollapse;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private Tiles _tiles;
    private Tiles _tiles2;

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        _tiles = new Tiles(Content.Load<Texture2D>("pacman_tiles"), 24, 24, 1, 1);
        _tiles2 = new Tiles(Content.Load<Texture2D>("pacman_sample_screen"), 8, 8, 1, 1);
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        // TODO: Add your update logic here

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        // TODO: Add your drawing code here
        _spriteBatch.Begin();
        
        _tiles2.Draw(_spriteBatch, new Rectangle(0, 0, 0, 0));
        
        _tiles.Draw(_spriteBatch, new Rectangle(0, 300, 0, 0));


        _spriteBatch.End();

        base.Draw(gameTime);
    }
}
