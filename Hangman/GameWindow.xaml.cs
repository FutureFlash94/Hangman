using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Hangman
{
    /// <summary>
    /// Interaktionslogik für GameWindow.xaml
    /// </summary>
    public partial class GameWindow : Window
    {
        private readonly List<TextBox> textBoxes = new List<TextBox>();
        private readonly List<Button> buttonList = new List<Button>();

        private enum Categories
        {
            Animal,
            Mathematics,
            Country
        };
        private readonly string[] AnimalList = new string[] {
            "AGAME", "ALPACA", "ANT", "ANTEATER", "BADGER", "BEAR", "BEAVER", "BEE",
            "LEECH", "BOA", "EAGLE", "EEL", "CHAMELEON", "CHINCHILLA", "CLOWFISH",
            "DEGU", "DOLPHIN", "LIZARD", "MOOSE", "ELEPHANT", "DUCK",
            "FALCON", "SLOTH", "FISH", "FLY", "BAT", "HIPPO", "FROG", "FOX",
            "SHARK", "HAMSTER", "RABBIT", "DEER", "LOBSTER", "DOG", "HUSKY",
            "HEDGEHOG", "DONKEY", "OWL", "BEETLE", "Cockatoo", "CAMEL", "KANGAROO",
            "RABBIT", "TOP-OVER", "CAT", "KOALA", "CROCODILE", "TOAD", "LAMA", "LION",
            "LYNX", "MOUSE", "RHINO", "OTTER", "PANDA", "HORSE", "RAT", "SPIDER",
            "TIGER", "Bird", "WORM", "ZEBRA"
        };
        private readonly string[] MathematicsList = new string[] {
            "DERIVATION", "ARGUMENT", "BASE", "AMOUNT", "BINOM", "FRACTION", "PI",
            "COSINE", "DIFFERENCE", "TRIANGLE", "PLANE", "ELEMENT", "RESULT",
            "FACTOR", "FACULTY", "AREA", "FORMULA", "GEOMETRY", "GRAPH", "SQUARE",
            "INTERVAL", "COMMA", "REVERSE", "SOLUTION", "MEDIUM", "ZERO", "OPERATION",
            "SINE", "VECTOR", "SQUARE ROOT", "ANGLE", "NUMBER"
        };
        private readonly string[] CountyList = new string[] {
            "AFRICA", "ALBANIA", "AUSTRALIA", "EGYPT", "BELGIA", "BOLIVIA",
            "BULGARIA", "CHILE", "DENMARK", "DUBAI", "ESTONIA", "FINLAND",
            "FRANCE", "GREECE", "GUYANA", "GREENLAND", "HAITI", "IRELAND",
            "ICELAND", "IRAQ", "IRAN", "JAPAN", "COLOMBIA", "CONGO", "KOSOVO",
            "CROATIA", "CUBA", "LIBYA", "LATVIA", "LITHUANIA", "LIBERIA", "LUXEMBOURG",
            "MEXICO", "MONACO", "MALTA", "NORWAY", "AUSTRIA", "PERU", "ROMANIA",
            "SPAIN", "SWEDEN", "TIBET", "UKRAINE", "HUNGARY", "USA", "WALES",
            "CYPRUS", "ZIMBABWE"
        };

        private static class TbForeground
        {
            public static SolidColorBrush DEFAULT = Brushes.White;
            public static SolidColorBrush RIGHT_CHAR = Brushes.Green;
            public static SolidColorBrush WRONG_CHAR = Brushes.Red;
        }
        private static class BtnBackground
        {
            public static SolidColorBrush DEFAULT = Brushes.LightGray;
            public static SolidColorBrush RIGHT_CHAR = Brushes.Green;
            public static SolidColorBrush WRONG_CHAR = Brushes.Red;
        }

        private string rand_word;

        public GameWindow()
        {
            InitializeComponent();

            // set version as text
            tb_version.Text = FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).FileVersion.ToString();

            textBoxes.Add(tb1);
            textBoxes.Add(tb2);
            textBoxes.Add(tb3);
            textBoxes.Add(tb4);
            textBoxes.Add(tb5);
            textBoxes.Add(tb6);
            textBoxes.Add(tb7);
            textBoxes.Add(tb8);
            textBoxes.Add(tb9);
            textBoxes.Add(tb10);

            buttonList.Add(btn_a);
            buttonList.Add(btn_b);
            buttonList.Add(btn_c);
            buttonList.Add(btn_d);
            buttonList.Add(btn_e);
            buttonList.Add(btn_f);
            buttonList.Add(btn_g);
            buttonList.Add(btn_h);
            buttonList.Add(btn_i);
            buttonList.Add(btn_j);
            buttonList.Add(btn_k);
            buttonList.Add(btn_l);
            buttonList.Add(btn_m);
            buttonList.Add(btn_n);
            buttonList.Add(btn_o);
            buttonList.Add(btn_p);
            buttonList.Add(btn_q);
            buttonList.Add(btn_r);
            buttonList.Add(btn_s);
            buttonList.Add(btn_t);
            buttonList.Add(btn_u);
            buttonList.Add(btn_v);
            buttonList.Add(btn_w);
            buttonList.Add(btn_x);
            buttonList.Add(btn_y);
            buttonList.Add(btn_z);

            CreateNewGame();
        }

        private void CreateNewGame()
        {
            // choose random category
            Random rand = new Random();
            int rand_category = rand.Next(0, Enum.GetNames(typeof(Categories)).Length);
            string cString = Enum.GetNames(typeof(Categories)).ElementAt(rand_category);
            tb_Category.Text = "Category: " + cString;

            // choose random word from category
            switch ((Categories)Enum.Parse(typeof(Categories), cString))
            {
                case Categories.Animal:
                    rand_word = AnimalList[rand.Next(0, AnimalList.Length)];
                    break;
                case Categories.Mathematics:
                    rand_word = MathematicsList[rand.Next(0, MathematicsList.Length)];
                    break;
                case Categories.Country:
                    rand_word = CountyList[rand.Next(0, CountyList.Length)];
                    break;
            }
            
            grid_newgame.Visibility = Visibility.Hidden;
            SetHangmanPicture("/assets/img/Hangman_0.png");

            foreach (Button btn in buttonList.Where(btn => btn.Background != BtnBackground.DEFAULT).ToList())
            {
                btn.Background = BtnBackground.DEFAULT;
                btn.IsEnabled = true;
            }

            // align TextBoxes for random word
            Thickness tb_margin = textBoxes[0].Margin;
            double rage = tb_Category.Width - imgHangman.Width;
            switch (rand_word.Length)
            {
                case 2: tb_margin.Left = Math.Round(rage / 2) - 45; break;
                case 3: tb_margin.Left = Math.Round(rage / 2) - 70; break;
                case 4: tb_margin.Left = Math.Round(rage / 2) - 45 - 50; break;
                case 5: tb_margin.Left = Math.Round(rage / 2) - 70 - 50; break;
                case 6: tb_margin.Left = Math.Round(rage / 2) - 45 - 100; break;
                case 7: tb_margin.Left = Math.Round(rage / 2) - 70 - 100; break;
                case 8: tb_margin.Left = Math.Round(rage / 2) - 45 - 150; break;
                case 9: tb_margin.Left = Math.Round(rage / 2) - 70 - 150; break;
                case 10: tb_margin.Left = Math.Round(rage / 2) - 45 - 200; break;
            }

            for (int tbIndex = 0; tbIndex < textBoxes.Count; tbIndex++)
            {
                if (tbIndex < rand_word.Length)
                {
                    // TextBox is needed for the game
                    textBoxes[tbIndex].Margin = tb_margin;
                    textBoxes[tbIndex].Visibility = Visibility.Visible;
                    textBoxes[tbIndex].Text = rand_word.ToCharArray()[tbIndex].ToString();
                    tb_margin.Left += 50;
                }
                else
                {
                    // TextBox is not needed for the game
                    textBoxes[tbIndex].Visibility = Visibility.Hidden;
                    textBoxes[tbIndex].Text = "";
                }
                textBoxes[tbIndex].Foreground = TbForeground.DEFAULT;
            }
        }

        private void SearchAlphabeticChar(char character)
        {
            int isCharInRandWord = rand_word.IndexOf(character, 0);
            int charAsInt = CharToInt(character);

            if (isCharInRandWord != -1)
            {
                while (isCharInRandWord != -1)
                {
                    textBoxes[isCharInRandWord].Foreground = TbForeground.RIGHT_CHAR;
                    buttonList[charAsInt].Background = BtnBackground.RIGHT_CHAR;
                    buttonList[charAsInt].BorderBrush = BtnBackground.RIGHT_CHAR;
                    isCharInRandWord = rand_word.IndexOf(character, isCharInRandWord + 1);
                }
                if (textBoxes.Where(tb => tb.Visibility == Visibility.Visible && tb.Foreground == TbForeground.DEFAULT).Count() == 0)
                {
                    WinGame();
                }
            }
            else
            {
                buttonList[charAsInt].Background = BtnBackground.WRONG_CHAR;
                buttonList[charAsInt].BorderBrush = BtnBackground.WRONG_CHAR;

                switch (buttonList.Where(btn => btn.Background == BtnBackground.WRONG_CHAR).Count())
                {
                    case 1: SetHangmanPicture("/assets/img/Hangman_1.png"); break;
                    case 2: SetHangmanPicture("/assets/img/Hangman_2.png"); break;
                    case 3: SetHangmanPicture("/assets/img/Hangman_3.png"); break;
                    case 4: SetHangmanPicture("/assets/img/Hangman_4.png"); break;
                    case 5: SetHangmanPicture("/assets/img/Hangman_5.png"); break;
                    case 6: SetHangmanPicture("/assets/img/Hangman_6.png"); LoseGame(); break;
                }
            }
        }

        private void WinGame()
        {
            tb_status.Text = "You won";
            grid_newgame.Visibility = Visibility.Visible;
        }

        private void LoseGame()
        {
            tb_status.Text = "You lose";
            grid_newgame.Visibility = Visibility.Visible;

            foreach (TextBox tb in textBoxes.Where(tb => tb.Visibility == Visibility.Visible && tb.Foreground == TbForeground.DEFAULT).ToList())
            {
                tb.Foreground = TbForeground.WRONG_CHAR;
            }
        }

        private void SetHangmanPicture(string path)
        {
            imgHangman.Source = new BitmapImage(new Uri(path, UriKind.Relative));
        }

        private int CharToInt(char character)
        {
            return char.ConvertToUtf32(character.ToString(), 0) - 65;
        }

        private void Btn_newgame_Click(object sender, RoutedEventArgs e)
        {
            CreateNewGame();
        }

        private void Btn_Click(object sender, RoutedEventArgs e)
        {
            Button clickedButton = (Button)sender;
            SearchAlphabeticChar(((string)clickedButton.Content).ElementAt(0));
            clickedButton.IsEnabled = false;
        }

        private void Game_Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (textBoxes.Where(tb => tb.Visibility == Visibility.Visible && tb.Foreground == TbForeground.DEFAULT).Count() == 0)
            {
                return; // Game is over
            }
            Button b = new Button
            {
                Content = e.Key.ToString()
            };
            switch (e.Key)
            {
                case Key.A:
                case Key.B:
                case Key.C:
                case Key.D:
                case Key.E:
                case Key.F:
                case Key.G:
                case Key.H:
                case Key.I:
                case Key.J:
                case Key.K:
                case Key.L:
                case Key.M:
                case Key.N:
                case Key.O:
                case Key.P:
                case Key.Q:
                case Key.R:
                case Key.S:
                case Key.T:
                case Key.U:
                case Key.V:
                case Key.W:
                case Key.X:
                case Key.Y:
                case Key.Z: Btn_Click(b, null); break;
                default: break;
            }
        }
    }
}
