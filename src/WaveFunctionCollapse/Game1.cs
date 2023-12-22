using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using WaveFunctionCollapse.Tiles;

namespace WaveFunctionCollapse;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private TileMaker _tiles;
    
    public static SpriteFont Font { get; private set; }

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        _tiles = new TileMaker(Content.Load<Texture2D>("pacman_sample_screen"), 8, 8);
        Font = Content.Load<SpriteFont>("ArialFont");
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);
        _spriteBatch.Begin();
        _tiles.Draw(_spriteBatch, new Rectangle(1, 1, 0, 0));
        _spriteBatch.End();
        base.Draw(gameTime);
    }
}
