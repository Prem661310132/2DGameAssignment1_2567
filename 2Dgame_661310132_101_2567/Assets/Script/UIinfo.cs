using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIinfo : MonoBehaviour
{
    
    public Image healthBar; // ᶺ��ѧ���Ե
    public TextMeshProUGUI lifeText; // �ʴ��ӹǹ���Ե
    public TextMeshProUGUI scoreText; // �ʴ���ṹ
    public PlayerControl playerControl; // ��ҧ�ԧ�֧ PlayerController
    public PlayerControl Enemies;
    [SerializeField] FloatingHeathBar healthbar;

    void Start()
    {
        // ��Ǩ�ͺ��� PlayerController �١��駤�����
        if (playerControl == null)
        {
            Debug.LogError("PlayerController is not assigned to UIinfo.");
        }
    }

    void Update()
    {
        if (playerControl != null)
        {

            float healthPercentage = playerControl.playerCurrentHealth / playerControl.playerMaxHealth;
            healthPercentage = Mathf.Clamp(healthPercentage, 0f, 1f); // �ӡѴ���㹪�ǧ 0 �֧ 1
            healthBar.fillAmount = healthPercentage;


            lifeText.text = "Life: " + playerControl.playerLife;

            
            scoreText.text = "Score: " + playerControl.score;
        }
    }


}
