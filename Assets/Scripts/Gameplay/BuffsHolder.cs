using System.Collections.Generic;
using UnityEngine;

namespace Gameplay
{
	[DisallowMultipleComponent]
	public class BuffsHolder : MonoBehaviour
	{
		private Dictionary<string, Buff> buffs = new Dictionary<string, Buff>();

		private Dictionary<string, Buff> currentBuffs = new Dictionary<string, Buff>();

		private List<string> keysToRemove = new List<string>();

		private List<string> keysList = new List<string>();

		public event BuffValueChangedHandler OnBuffValueChanged;

		public bool ContainsBuffKey(string buffKey)
		{
			return buffs.ContainsKey(buffKey);
		}

		public bool TryGetBuffValue(string buffKey, out float buffValue)
		{
			if (buffs.TryGetValue(buffKey, out Buff value))
			{
				buffValue = value.Value;
				return true;
			}
			buffValue = 0f;
			return false;
		}

		public float GetBuffsValue(List<string> buffKeys)
		{
			float num = 0f;
			for (int i = 0; i < buffKeys.Count; i++)
			{
				if (buffs.TryGetValue(buffKeys[i], out Buff value))
				{
					num += value.Value;
				}
			}
			return num;
		}

		public void AddBuff(string buffKey, Buff buff, BuffStackLogic valueStackLogic, BuffStackLogic timeStackLogic)
		{
			if (buffs.TryGetValue(buffKey, out Buff value))
			{
				value.Value = BuffStackLogicHelper.GetStackedValue(valueStackLogic, value.Value, buff.Value);
				value.Duration = BuffStackLogicHelper.GetStackedValue(timeStackLogic, value.Duration, buff.Duration);
			}
			else
			{
				value = buff;
			}
			buffs[buffKey] = value;
			if (!keysList.Contains(buffKey))
			{
				keysList.Add(buffKey);
			}
			if (this.OnBuffValueChanged != null)
			{
				this.OnBuffValueChanged(buffKey, added: true);
			}
		}

		public void ResetBuffs()
		{
			buffs.Clear();
			keysToRemove.Clear();
			keysList.Clear();
		}

		public void RemoveBuffs(IEnumerable<string> buffKeys)
		{
			_RemoveBuffs(buffKeys);
		}

		public void RemoveBuffs(params string[] buffKeys)
		{
			_RemoveBuffs(buffKeys);
		}

		public void RemoveBuffs(bool isPositive)
		{
			List<string> list = new List<string>();
			foreach (KeyValuePair<string, Buff> buff in buffs)
			{
				if (buff.Value.IsPositive == isPositive)
				{
					list.Add(buff.Key);
				}
			}
			RemoveBuffs(list);
		}

		public void CopyBuff(BuffsHolder targetBuffsHolder)
		{
			targetBuffsHolder.ResetBuffs();
			foreach (KeyValuePair<string, Buff> buff in buffs)
			{
				targetBuffsHolder.AddBuff(buff.Key, buff.Value, BuffStackLogic.ChooseMax, BuffStackLogic.ChooseMax);
			}
		}

		public void Update()
		{
			UpdateBuffsTime();
			RemoveTimedOutBuffs();
		}

		private void _RemoveBuffs(IEnumerable<string> buffKeys)
		{
			foreach (string buffKey in buffKeys)
			{
				buffs.Remove(buffKey);
				keysList.Remove(buffKey);
				if (this.OnBuffValueChanged != null)
				{
					this.OnBuffValueChanged(buffKey, added: false);
				}
			}
		}

		private void UpdateBuffsTime()
		{
			if (buffs.Count == 0)
			{
				return;
			}
			for (int i = 0; i < keysList.Count; i++)
			{
				string text = keysList[i];
				Buff value = buffs[text];
				if (value.Duration <= 0f)
				{
					keysToRemove.Add(text);
				}
				else
				{
					value.Duration -= Time.deltaTime;
				}
				buffs[text] = value;
			}
		}

		private void RemoveTimedOutBuffs()
		{
			if (keysToRemove.Count != 0)
			{
				RemoveBuffs(keysToRemove);
				keysToRemove.Clear();
			}
		}
	}
}
