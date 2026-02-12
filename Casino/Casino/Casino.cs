using System;
using Casino.Core;
using Casino.Games.BlackJack;
using Casino.Games.DiceGame;
using Casino.Models;
using Casino.Services;

namespace Casino
{
    public class Casino : IGame
    {
        private const decimal MaxBank = 1000m;

        private readonly ISaveLoadService<string> _saveService;

        private readonly BlackJackGame _blackJackGame;
        private readonly DiceGame _diceGame;

        
        private PlayerProfile _player = new PlayerProfile { Name = string.Empty, Bank = 0m };
        private decimal _currentBet;

        public Casino()
        {
            _saveService = new FileSystemSaveLoadService("Saves");

            _blackJackGame = new BlackJackGame(36);
            _diceGame = new DiceGame(2, 1, 6);

            SubscribeToEvents(_blackJackGame);
            SubscribeToEvents(_diceGame);
        }
        private CasinoGameBase? GetGame(string? choice)
        {
            return choice switch
            {
                "1" => _blackJackGame,
                "2" => _diceGame,
                _ => null
            };
        }

        private void ChangeProfile()
        {
            Console.WriteLine("Enter new profile name:");

            string newName = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(newName))
            {
                Console.WriteLine("Invalid name.");
                return;
            }

            string data = _saveService.LoadData(newName);

            if (string.IsNullOrEmpty(data))
            {
                Console.WriteLine("Profile not found. Creating new profile.");

                _player = new PlayerProfile
                {
                    Name = newName,
                    Bank = 500
                };
            }
            else
            {
                string[] parts = data.Split('|');

                _player = new PlayerProfile
                {
                    Name = parts[0],
                    Bank = decimal.Parse(parts[1])
                };

                Console.WriteLine("Profile loaded successfully.");
            }
        }
        public void StartGame()
        {
            Console.WriteLine("Welcome to the Casino!");

            LoadProfile();

            while (true)
            {
                if (_player.Bank <= 0)
                {
                    Console.WriteLine("No money? Kicked!");
                    break;
                }

                Console.WriteLine();
                Console.WriteLine($"Your bank: {_player.Bank}");                
                Console.WriteLine("Choose option:");
                Console.WriteLine("1 - BlackJack");
                Console.WriteLine("2 - Dice");
                Console.WriteLine("3 - Change profile");
                Console.WriteLine("0 - Exit");


                string? choice = Console.ReadLine();

                if (choice == "0")
                {
                    break;
                }

                if (choice == "3")
                {
                    ChangeProfile();
                    continue;
                }

                CasinoGameBase? selectedGame = GetGame(choice);


                if (selectedGame == null)
                {
                    Console.WriteLine("Wrong selection.");
                    continue;
                }

                if (!MakeBet())
                {
                    continue;
                }

                selectedGame.PlayGame();

                ProcessBankLimits();

                Console.WriteLine($"Current bank: {_player.Bank}");
            }

            Console.WriteLine("Goodbye!");
            SaveProfile();
        }   

        private bool MakeBet()
        {
            Console.WriteLine("Enter your bet:");

            string? input = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(input) || !decimal.TryParse(input, out decimal bet))
            {
                Console.WriteLine("Invalid bet.");
                return false;
            }

            if (bet <= 0 || bet > _player.Bank)
            {
                Console.WriteLine("Bet cannot exceed your bank.");
                return false;
            }

            _currentBet = bet;
            return true;
        }


        private void SubscribeToEvents(CasinoGameBase game)
        {
            game.OnWin += OnPlayerWin;
            game.OnLoose += OnPlayerLoose;
            game.OnDraw += OnDraw;
        }

        private void OnPlayerWin()
        {
            _player.Bank += _currentBet;

            if (_player.Bank > MaxBank)
            {
                decimal overflow = _player.Bank - MaxBank;
                _player.Bank = MaxBank;

                Console.WriteLine("You broke the casino! A new one will be built here.");
                Console.WriteLine($"Extra money returned: {overflow}");
            }
        }

        private void OnPlayerLoose()
        {
            _player.Bank -= _currentBet;

            if (_player.Bank <= 0)
            {
                _player.Bank = 0;
                Console.WriteLine("No money? Kicked!");
            }
        }

        private void OnDraw()
        {
            Console.WriteLine("Bet returned.");
        }

        private void ProcessBankLimits()
        {
            if (_player.Bank > MaxBank)
            {
                _player.Bank /= 2;
                Console.WriteLine("You wasted half of your bank money in casino’s bar");
            }
        }

        private void LoadProfile()
        {
            Console.WriteLine("Enter profile name:");
            string name = Console.ReadLine();

            string data = _saveService.LoadData(name);

            if (string.IsNullOrEmpty(data))
            {
                Console.WriteLine("Profile not found. Creating new profile.");

                _player = new PlayerProfile
                {
                    Name = name,
                    Bank = 500
                };
            }
            else
            {
                string[] parts = data.Split('|');

                _player = new PlayerProfile
                {
                    Name = parts[0],
                    Bank = decimal.Parse(parts[1])
                };

                Console.WriteLine($"Welcome back, {_player.Name}!");
            }
        }

        private void SaveProfile()
        {
            string data = $"{_player.Name}|{_player.Bank}";
            _saveService.SaveData(data, _player.Name);
        }
    }
}