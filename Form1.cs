using System.Drawing.Text;
using System.Media;

namespace reversi
{
    public partial class Form1 : Form
    {
        int rows = 8;
        int cols = 8;

        // in pixels, the Y offset is very strange for some reason
        int xOffset = 400;
        int yOffset = 70;
        int buttonSize = 110;

        string s_blackImg;
        string s_whiteImg;
        string s_clickSound;

        int label_xOffset = 100;
        int label_yOffset = 200;

        bool isPvCPU = false;

        private ReversiGame game;

        public ReversiButton[,] buttons;

        private Label blackScore;
        private Label whiteScore;

        public Form1()
        {
            string baseDir = AppDomain.CurrentDomain.BaseDirectory; //????????
            s_blackImg = Path.Combine(baseDir, "graphics", "reversi_black.png");
            s_whiteImg = Path.Combine(baseDir, "graphics", "reversi_white.png");
            s_clickSound = Path.Combine(baseDir, "sounds", "click.wav");

            PrivateFontCollection fontCollection = FontLoader.LoadEmbeddedFont("reversi.TempleOS.ttf");
            Font customFont = new Font(fontCollection.Families[0], 24);

            InitializeComponent();
            
            buttons = new ReversiButton[rows, cols];
            blackScore = new Label
            {
                Location = new Point(rows * buttonSize + xOffset + label_xOffset, label_yOffset),
                Size = new Size(500, 100),
                ForeColor = Color.Black,
                //Font = new Font("TempleOS", 24)
            };
            whiteScore = new Label
            {
                Location = new Point(rows * buttonSize + xOffset + label_xOffset, label_yOffset * 2),
                Size = new Size(500, 100),
                ForeColor = Color.White,
                //Font = new Font("TempleOS", 24)
            };
            blackScore.Font = customFont;
            whiteScore.Font = customFont;
        }
        private void HideMenu()
        {
            title.Visible = false;
            PvP.Visible = false;
            PvCPU.Visible = false;
        }
        private void Generate_Grid()
        {
            int gridWidth = cols * buttonSize;
            int gridHeight = rows * buttonSize;

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    ReversiButton button = new ReversiButton
                    {
                        Width = buttonSize,
                        Height = buttonSize,
                        Location = new Point(j * buttonSize + xOffset, i * buttonSize + yOffset),
                        Name = $"Button_{i}_{j}",
                        BackColor = Color.White,
                        GridX = i,
                        GridY = j
                    };
                    buttons[i, j] = button;
                    button.Click += Button_Click;

                    //button.State = ReversiState.Empty;

                    this.Controls.Add(button);
                }
            }
            // Set initial state of the center buttons
            //buttons[3, 3].State = ReversiState.White;
            //buttons[3, 4].State = ReversiState.Black;
            //buttons[4, 3].State = ReversiState.Black;
            //buttons[4, 4].State = ReversiState.White;

            game = new ReversiGame(s_clickSound);

            this.Controls.Add(blackScore);
            this.Controls.Add(whiteScore);

            Button SolveButton = new Button
            {
                Text = "Solve",
                Location = new Point(rows * buttonSize + xOffset, label_yOffset * 3),
                Size = new Size(100, 50)
            };
            SolveButton.Click += SolveTheGrid;

            this.Controls.Add(SolveButton);

            Refresh_Board();

        }
        // pvp game loop
        private void PvP_Click(object sender, EventArgs e)
        {
            HideMenu();
            Generate_Grid();
        }
        // player vs cpu game loop
        private void PvCPU_Click(object sender, EventArgs e)
        {
            HideMenu();
            Generate_Grid();
            isPvCPU = true;
        }
        
        private async void Button_Click(object sender, EventArgs e)
        {
            var btn = sender as ReversiButton;



            game.ApplyMove(btn.GridX, btn.GridY);
            Refresh_Board();
            // cpu moves
            if (isPvCPU && game.CurrentPlayer == ReversiGame.White && !game.IsGameOver())
            {
                var aiMove = game.GetRandomAIMove(ReversiGame.White);
                if (aiMove.HasValue) // because this can be null
                {
                    await Task.Delay(500);
                    game.ApplyMove(aiMove.Value.x, aiMove.Value.y);
                    Refresh_Board();
                }
            }
            Refresh_Board();

            // Highlight valid moves, doesnt highlight the starting click
            var validMoves = game.GetValidMoves(game.CurrentPlayer);
            foreach (var (x, y) in validMoves)
            {
                var hlButton = buttons[x, y];
                hlButton.BackColor = Color.LightGreen;
            }

        }
        private void Refresh_Board()
        {
            for (int x = 0; x < 8; x++)
            {
                for (int y = 0; y < 8; y++)
                {
                    int value = game.Board[x, y];
                    var btn = buttons[x, y];
                    btn.BackColor = Color.White;

                    if (value == ReversiGame.Black)
                        btn.Image = Image.FromFile(s_blackImg);
                    else if (value == ReversiGame.White)
                        btn.Image = Image.FromFile(s_whiteImg);
                    else
                        btn.Image = null;

                    btn.State = (ReversiState)value;
                }
            }

            

            // Update scores
            blackScore.Text = $"Black score: {game.GetScore().black}";
            whiteScore.Text = $"White score: {game.GetScore().white}";

            if (game.IsGameOver())
            {
                string winner = game.GetWinner();
                var result = MessageBox.Show($"Game Over! {winner}\nDo you want to restart?", "The end", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {
                    game.Restart();
                    Refresh_Board();
                }
                else
                {
                    Application.Exit();
                }
            }
        }



        // debug from chatgpt
        public void SolveTheGrid(object sender, EventArgs e)
        {
            Random rand = new Random();
            // Wyczyœæ planszê
            for (int x = 0; x < rows; x++)
                for (int y = 0; y < cols; y++)
                    game.Board[x, y] = ReversiGame.Empty;

            // Losowo rozmieœæ czarne i bia³e pionki (np. po 32 ka¿dego)
            int blackCount = 0, whiteCount = 0, max = 32;
            while (blackCount < max)
            {
                int x = rand.Next(rows);
                int y = rand.Next(cols);
                if (game.Board[x, y] == ReversiGame.Empty)
                {
                    game.Board[x, y] = ReversiGame.Black;
                    blackCount++;
                }
            }
            while (whiteCount < max)
            {
                int x = rand.Next(rows);
                int y = rand.Next(cols);
                if (game.Board[x, y] == ReversiGame.Empty)
                {
                    game.Board[x, y] = ReversiGame.White;
                    whiteCount++;
                }
            }
            game.CurrentPlayer = ReversiGame.Black;
            Refresh_Board();
        }
    }
}
