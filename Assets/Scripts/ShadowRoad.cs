using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowRoad : MonoBehaviour
{
	public bool isAir;

	public GameObject note;

	public GameObject path;

	[Range(-99f, 1000f)]
	public float deltaDistanceBetweenNotesPercent;

	public float speed = 0.2f;

	private int notesSize;

	private float noteLength;

	private List<GameObject> listNotes = new List<GameObject>();

	private void Start()
	{
		Invoke("IERun", 0.5f);
	}

	private void IERun()
	{
		Run(haveFlyEnemy: true);
	}

	public void Run(bool haveFlyEnemy)
	{
		if (haveFlyEnemy || !isAir)
		{
			SetListNote();
			DOTweenPath component = path.GetComponent<DOTweenPath>();
			float num = 1f / speed;
			float num2 = num / (float)listNotes.Count;
			for (int i = 0; i < listNotes.Count; i++)
			{
				GameObject gameObject = listNotes[i];
				gameObject.transform.position = path.transform.position;
				gameObject.transform.DOPath(component.wps.ToArray(), num, PathType.CatmullRom, PathMode.TopDown2D).SetLookAt(0f);
				StartCoroutine(InvokeKill((float)(i + 1) * num2, gameObject));
			}
		}
	}

	private void SetListNote()
	{
		if (listNotes.Count > 0)
		{
			ResetListNotes();
			StopCoroutine("InvokeKill");
			return;
		}
		SetNotesNumber();
		listNotes.Clear();
		listNotes.Add(note);
		for (int i = 1; i < notesSize; i++)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate(note);
			gameObject.name = "note" + i;
			gameObject.transform.position = note.transform.position;
			gameObject.transform.parent = note.transform.parent;
			gameObject.transform.localScale = note.transform.localScale;
			listNotes.Add(gameObject);
		}
	}

	private IEnumerator InvokeKill(float time, GameObject obj)
	{
		yield return new WaitForSeconds(time);
		obj.GetComponent<SpriteRenderer>().enabled = true;
		obj.transform.DOKill();
	}

	private void SetNoteWidth()
	{
		Vector3 size = note.GetComponent<SpriteRenderer>().bounds.size;
		float x = size.x;
		Vector3 localScale = base.transform.localScale;
		noteLength = x * localScale.x;
		noteLength += deltaDistanceBetweenNotesPercent * noteLength / 100f;
	}

	private void ResetListNotes()
	{
		foreach (GameObject listNote in listNotes)
		{
			if (listNote != note)
			{
				listNote.GetComponent<SpriteRenderer>().enabled = false;
				listNote.GetComponent<SpriteRenderer>().color = Color.white;
				listNote.transform.DOKill();
			}
		}
	}

	private void SetNotesNumber()
	{
		SetNoteWidth();
		float num = path.GetComponent<DOTweenPath>().tween.PathLength();
		notesSize = (int)(num / noteLength);
	}

	public void Hide()
	{
		int count = listNotes.Count;
		float num = 3f / (float)count;
		float duration = 0.5f;
		for (int i = 0; i < count - 1; i++)
		{
			GameObject gameObject = listNotes[i];
			gameObject.GetComponent<SpriteRenderer>().DOFade(0f, duration).SetEase(Ease.OutExpo)
				.SetDelay(num * (float)i);
		}
		listNotes[count - 1].GetComponent<SpriteRenderer>().DOFade(0f, duration).SetEase(Ease.OutExpo)
			.SetDelay(num * (float)(count - 1))
			.OnComplete(ResetListNotes);
	}

	public void Reload()
	{
		ResetListNotes();
	}
}
