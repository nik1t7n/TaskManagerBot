using System;
using System.Collections.Generic;
using TelegramBot.Entities;
using TelegramBot.Data;
using TaskHelperBot.Services.Contracts;

namespace TaskHelperBot.Services.Implementations
{
    public class ChallengeService : IChallengeService
    {
        private readonly DataContext _context;

        public ChallengeService(DataContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context), "DataContext cannot be null.");
        }

        public Challenge AddChallenge(Challenge challenge)
        {
            if (challenge == null)
                throw new ArgumentNullException(nameof(challenge), "Challenge object cannot be null.");

            _context.AddChallenge(challenge);
            return challenge;
        }

        public void DeleteChallenge(int challengeId)
        {
            _context.DeleteChallenge(challengeId);
        }

        public List<Challenge> GetAllChallenges()
        {
            return _context.GetAllChallenges() as List<Challenge>;
        }

        public Challenge GetChallengeById(int challengeId)
        {
            return _context.GetChallengeById(challengeId);
        }

        public void UpdateChallenge(Challenge challenge)
        {
            if (challenge == null)
                throw new ArgumentNullException(nameof(challenge), "Challenge object cannot be null.");

            _context.UpdateChallenge(challenge);
        }
    }
}
