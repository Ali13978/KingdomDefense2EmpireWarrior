using System.Collections.Generic;
using UnityEngine;

namespace Gameplay
{
	public class PointPatternsGroup : MonoBehaviour
	{
		public PointPattern _3pointPattern;

		public PointPattern _4pointPattern;

		public PointPattern _5pointPattern;

		public List<Transform> getPoints(int pointAmount)
		{
			List<Transform> list = new List<Transform>();
			switch (pointAmount)
			{
			case 3:
			{
				foreach (Transform point in _3pointPattern.points)
				{
					list.Add(point);
				}
				return list;
			}
			case 4:
			{
				foreach (Transform point2 in _4pointPattern.points)
				{
					list.Add(point2);
				}
				return list;
			}
			case 5:
			{
				foreach (Transform point3 in _5pointPattern.points)
				{
					list.Add(point3);
				}
				return list;
			}
			default:
				return list;
			}
		}

		public Vector2 getParentPointsPosition(int pointAmount)
		{
			Vector2 result = Vector2.zero;
			switch (pointAmount)
			{
			case 3:
				result = _3pointPattern.transform.position;
				break;
			case 4:
				result = _4pointPattern.transform.position;
				break;
			case 5:
				result = _5pointPattern.transform.position;
				break;
			}
			return result;
		}
	}
}
