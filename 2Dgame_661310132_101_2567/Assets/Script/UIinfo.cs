using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIinfo : MonoBehaviour
{
    
    public Image healthBar; // แถบพลังชีวิต
    public TextMeshProUGUI lifeText; // แสดงจำนวนชีวิต
    public TextMeshProUGUI scoreText; // แสดงคะแนน
    public PlayerControl playerControl; // อ้างอิงถึง PlayerController
    public PlayerControl Enemies;
    [SerializeField] FloatingHeathBar healthbar;

    void Start()
    {
        // ตรวจสอบว่า PlayerController ถูกตั้งค่าไว้
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
            healthPercentage = Mathf.Clamp(healthPercentage, 0f, 1f); // จำกัดค่าในช่วง 0 ถึง 1
            healthBar.fillAmount = healthPercentage;


            lifeText.text = "Life: " + playerControl.playerLife;

            
            scoreText.text = "Score: " + playerControl.score;
        }
    }


}
