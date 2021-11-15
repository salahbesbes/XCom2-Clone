using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class StatsHolder : ObserverAbstraction<PlayerStats, PlayerStats>
{
	public float chipSpeed;
	private float lerpSpeed;

	public Image frontHealthBar;
	public Image backHealthBar;
	public GameObject healthUnit;
	public Image frameHealthBar;
	private int maxHealthUnit = 0;

	public Text actionPointText;
	public Text HealthText;

	public override void OnSubjectEventChanges(PlayerStats Subject)
	{
		HealthText.text = $"{SubjectRef.Health}";
		updateHealthBar();
		actionPointText.text = $"{SubjectRef.ActionPoint}";
	}

	private void Start()
	{
		//float unitWidth = healthUnit.transform.GetComponent<Image>().rectTransform.rect.width;
		//float len = 0;
		//for (int i = 0; i < SubjectRef.Health; i++)
		//{
		//	GameObject unit = Instantiate(healthUnit, frontHealthBar.transform.position, frameHealthBar.transform.rotation);
		//	maxHealthUnit++;
		//	len += unitWidth + frontHealthBar.GetComponent<HorizontalLayoutGroup>().spacing;
		//	unit.transform.position = new Vector3(transform.position.x, transform.position.y, 0);
		//	unit.transform.SetParent(frontHealthBar.transform);
		//	if (len + unitWidth >= frontHealthBar.rectTransform.rect.width)
		//		break;
		//}
	}

	private IEnumerator updateHealthBarProgressively(Image image, float currentValue, float destination)
	{
		while (true)
		{
			yield return null;

			//Debug.Log($"image.fill {image.fillAmount},dest {destination}, curentVal {currentValue} ");
			// this represent the timer it gets updated each frame
			lerpSpeed += Time.deltaTime;
			float percentComplete = lerpSpeed / chipSpeed;
			percentComplete *= percentComplete;
			// this is [0,1] value used to execute the mathf lerp method
			image.fillAmount = Mathf.Lerp(currentValue, destination, percentComplete);
			yield return null;
			// if we the percent reaches 1 => finish moving => reset the counter timer
			// and breal the loop

			if (percentComplete >= 1)
			{
				lerpSpeed = 0;
				// reset the speed lirp
				break;
			}
		}
	}

	private IEnumerator updateHealthUnitProgressively()
	{
		yield return null;
		int RestHealthUnit = Mathf.FloorToInt(SubjectRef.Health * maxHealthUnit / SubjectRef.MaxHealth);
		for (int i = 0; i <= RestHealthUnit; i++)
		{
			if (frontHealthBar.transform.childCount == i) break;

			frontHealthBar.transform.GetChild(i).gameObject.SetActive(true);
			//yield return new WaitForSeconds(0.1f);
			yield return null;
		}
	}

	public void updateHealthBar()
	{
		float fillA = frontHealthBar.fillAmount;
		float fillB = backHealthBar.fillAmount;
		// percent between [0,1]
		float newHealthPercent = SubjectRef.Health / SubjectRef.MaxHealth;
		StopCoroutine("updateHealthBarProgressively");
		if (fillB > newHealthPercent)
		{
			// taking damage
			frontHealthBar.fillAmount = newHealthPercent;
			backHealthBar.color = Color.red;

			int RestHealthUnit = Mathf.FloorToInt(SubjectRef.Health * maxHealthUnit / SubjectRef.MaxHealth);
			//for (int i = frontHealthBar.transform.childCount - 1; i > RestHealthUnit; i--)
			//{
			//	frontHealthBar.transform.GetChild(i).gameObject.SetActive(false);
			//}

			// we want to move to backHealthBar fill value to some other value ( less )
			StartCoroutine(updateHealthBarProgressively(backHealthBar, fillB, newHealthPercent));
		}
		else if (newHealthPercent > fillA)
		{
			// healing
			backHealthBar.color = Color.green;
			backHealthBar.fillAmount = newHealthPercent;
			StartCoroutine(updateHealthBarProgressively(frontHealthBar, fillA, newHealthPercent));
			//StartCoroutine(updateHealthUnitProgressively());
		}
	}
}