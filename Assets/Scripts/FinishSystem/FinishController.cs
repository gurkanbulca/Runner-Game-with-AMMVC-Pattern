using System.Collections.Generic;
using UnityEngine;

public class FinishController : ElementOf<GameManager>
{
    [SerializeField] private List<Transform> prizes;
    [SerializeField] private int normalPrize,bigPrize;
    [SerializeField] private ParticleSystem[] confettiParticles;
    [SerializeField] private PlayerData playerData;

    public List<Transform> Prizes => prizes;
    public EarnedPrize earnedPrize { get; private set; }

    private void Start()
    {
        Master.OnPrizeClaimed += HandlePrizeClaim;
        earnedPrize = new EarnedPrize();
    }

    /// <summary>
    /// On prize claimed, destroys prize object and added earned currency to the player data.
    /// </summary>
    /// <param name="prize"></param>
    private void HandlePrizeClaim(Transform prize)
    {
        
        if (prizes.Remove(prize))
        {
            Destroy(prize.gameObject);
            if (prizes.Count > 0)
            {
                earnedPrize.NormalPrize += normalPrize;
                playerData.currencyAmount += normalPrize;
            }
            else
            {
                earnedPrize.BigPrize += bigPrize;
                playerData.currencyAmount += bigPrize;
            }
        }
    }

    /// <summary>
    /// on finish line reached by players. activates confetti particles and translate game state to win state on game manager.
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        Master.WinGame();
        foreach (var particle in confettiParticles)
        {
            particle.gameObject.SetActive(true);
        }
    }

    public class EarnedPrize
    {
        public int NormalPrize;
        public int BigPrize;

    }
}
