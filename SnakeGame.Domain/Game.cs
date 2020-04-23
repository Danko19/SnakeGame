using System;
using System.Collections.Generic;
using System.Linq;

namespace SnakeGame.Domain
{
    public class Game
    {
        public Map Map { get; }
        public List<Snake> Snakes { get; }
        private readonly Random random = new Random();

        public Game(Map map, List<Snake> snakes)
        {
            Map = map;
            Snakes = snakes;
        }

        public void Tick()
        {
            Snakes.ForEach(s => s.Move(Map));
            var winners = Map
                .Where(s => s.Creatures.Cast<Snake>().Count() > 2)
                .Select(ResolveConflict);
            var winner = winners
                .GroupBy(x => x)
                .OrderByDescending(x => x.Count())
                .FirstOrDefault()?.Key;
            if (winner != null)
                throw new EndGameException(winner);
        }

        private Snake ResolveConflict(Cell conflictCell)
        {
            var conflictSnakes = conflictCell.Creatures.Cast<Snake>().ToList();
            var nonConflictSnakes = Snakes.Where(s => !conflictSnakes.Contains(s)).ToList();

            if (nonConflictSnakes.Count == 1)
                return nonConflictSnakes.Single();

            var okSnakes = conflictSnakes
                .Where(x => x.Body.First() != conflictCell)
                .ToList();

            if (okSnakes.Count == 1)
                return okSnakes.Single();

            return conflictSnakes.OrderByDescending(s => s.Body.Count).First();
        }

        public void GenerateFood()
        {
            var emptyCells = Map.Where(x => x.IsEmpty).ToList();
            var randomIndex = random.Next(emptyCells.Count);
            emptyCells[randomIndex].Creatures.Add(new Food());
        }
    }
}