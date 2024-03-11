using System.Collections.Generic;
using TelegramBot.Entities;

namespace TaskHelperBot.Services.Contracts
{
    public interface IChallengeService
    {
        // Метод для добавления нового вызова (задачи)
        Challenge AddChallenge(Challenge challenge);

        // Метод для получения списка всех вызовов (задач)
        List<Challenge> GetAllChallenges();

        // Метод для получения вызова (задачи) по идентификатору
        Challenge GetChallengeById(int challengeId);

        // Метод для обновления вызова (задачи)
        void UpdateChallenge(Challenge challenge);

        // Метод для удаления вызова (задачи)
        void DeleteChallenge(int challengeId);
    }
}
