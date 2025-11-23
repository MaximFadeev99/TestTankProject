using System;
using MessagePipe;
using TestTankProject.Runtime.MainMenu;
using TestTankProject.Runtime.UI.MainMenu;
using TMPro;
using UnityEngine;
using VContainer;

namespace TestTankProject.Runtime.UI.Scoreboard
{
    public class ScoreboardView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _currentPoints;
        [SerializeField] private TMP_Text _basePoints;
        [SerializeField] private TMP_Text _bonusPoints;
        [SerializeField] private TMP_Text _matches;
        [SerializeField] private TMP_Text _totalMatchAttempts;
        
        private IDisposable _disposableForSubscriptions;
        
        [Inject]
        private void Initialize(ISubscriber<UpdateScoreboard> setUpSubscriber)
        {
            DisposableBagBuilder bagBuilder = DisposableBag.CreateBuilder();
            setUpSubscriber.Subscribe(OnUpdateCommand).AddTo(bagBuilder);
            _disposableForSubscriptions = bagBuilder.Build();
        }

        private void OnUpdateCommand(UpdateScoreboard updateCommand)
        {
            _currentPoints.text = updateCommand.CurrentPoints.ToString();
            _basePoints.text = updateCommand.BasePoints.ToString();
            _bonusPoints.text = updateCommand.BonusPoints.ToString();
            _matches.text = updateCommand.CurrentMatches.ToString();
            _totalMatchAttempts.text = updateCommand.TotalMatchAttempts.ToString();
        }

        private void OnDestroy()
        {
            _disposableForSubscriptions?.Dispose();
        }
    }
}
