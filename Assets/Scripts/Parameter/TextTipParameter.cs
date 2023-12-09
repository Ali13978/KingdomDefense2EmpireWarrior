using System.Collections.Generic;
using UnityEngine;

namespace Parameter
{
	public class TextTipParameter : Singleton<TextTipParameter>
	{
		public List<List<TextTip>> listTextTip = new List<List<TextTip>>();

		public void ClearData()
		{
			listTextTip.Clear();
		}

		public void SetTextTipParameter(TextTip textTip)
		{
			int count = listTextTip.Count;
			if (count <= textTip.level)
			{
				List<TextTip> list = new List<TextTip>();
				list.Insert(textTip.id, textTip);
				listTextTip.Insert(textTip.level, list);
			}
			else
			{
				List<TextTip> list2 = listTextTip[textTip.level];
				list2.Insert(textTip.id, textTip);
			}
		}

		public TextTip GetTextTipParameter(int level, int id)
		{
			if (CheckParameter(level, id))
			{
				return listTextTip[level][id];
			}
			return default(TextTip);
		}

		private bool CheckParameter(int level, int id)
		{
			if (level > GetNumberOfLevel() || id >= GetNumberOfLevel())
			{
				return false;
			}
			return true;
		}

		public int GetNumberOfLevel()
		{
			return listTextTip.Count;
		}

		public int GetNumberTextTipByLevel(int level)
		{
			if (GetNumberOfLevel() > 0)
			{
				return listTextTip[level].Count;
			}
			return 0;
		}

		public string GetRandomTextTipContent(int level)
		{
			string empty = string.Empty;
			int index = Random.Range(0, GetNumberTextTipByLevel(level));
			TextTip textTip = listTextTip[level][index];
			return textTip.textTipContent;
		}
	}
}
