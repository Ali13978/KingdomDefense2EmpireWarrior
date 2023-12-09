using UnityEngine;

namespace Gameplay
{
	public class BuffKeyAttribute : PropertyAttribute
	{
		public enum KeyType
		{
			ToEnemy,
			ToTower
		}

		private KeyType keyType;

		public BuffKeyAttribute(KeyType keyType)
		{
			this.keyType = keyType;
		}

		public KeyType GetKeyType()
		{
			return keyType;
		}
	}
}
