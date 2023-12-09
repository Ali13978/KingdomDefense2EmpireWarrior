using System;
using UnityEngine;

namespace Gameplay
{
	public class MonsterPathData
	{
		public int curLineIndex;

		public MonsterPathAnchor firstAnchor;

		public MonsterPathAnchor secondAnchor;

		public Vector3 offset = Vector3.zero;

		public float offsetProportion;

		public Action OnCompletePath;

		public MonsterPathData(int lineIndex, Action OnCompletePath = null)
		{
			this.OnCompletePath = OnCompletePath;
			curLineIndex = lineIndex;
			firstAnchor = new MonsterPathAnchor(curLineIndex, 0, 0f);
			secondAnchor = new MonsterPathAnchor(curLineIndex, 0, 0f);
			LineManager.Current.MoveMonsterAnchor(firstAnchor, -0.4f);
			LineManager.Current.MoveMonsterAnchor(secondAnchor, 0.4f);
		}

		public MonsterPathData(Vector3 curPos, Action OnCompletePath = null)
		{
			this.OnCompletePath = OnCompletePath;
			LineManager.Current.FindNearestLine(curPos, out curLineIndex, out offset, out MonsterPathAnchor anchorOnSegment);
			firstAnchor = new MonsterPathAnchor(curLineIndex, anchorOnSegment.pathSegmentId, anchorOnSegment.lenProgress);
			secondAnchor = new MonsterPathAnchor(curLineIndex, anchorOnSegment.pathSegmentId, anchorOnSegment.lenProgress);
			LineManager.Current.MoveMonsterAnchor(firstAnchor, -0.4f);
			LineManager.Current.MoveMonsterAnchor(secondAnchor, 0.4f);
		}

		public MonsterPathData(MonsterPathAnchor pos, Action OnCompletePath = null)
		{
			this.OnCompletePath = OnCompletePath;
			curLineIndex = pos.curLineIndex;
			offset = Vector3.zero;
			firstAnchor = new MonsterPathAnchor(curLineIndex, pos.pathSegmentId, pos.lenProgress);
			secondAnchor = new MonsterPathAnchor(curLineIndex, pos.pathSegmentId, pos.lenProgress);
			LineManager.Current.MoveMonsterAnchor(firstAnchor, -0.4f);
			LineManager.Current.MoveMonsterAnchor(secondAnchor, 0.4f);
		}

		public MonsterPathData(int lineIndex, Vector3 curPos, Action OnCompletePath = null)
		{
			this.OnCompletePath = OnCompletePath;
			curLineIndex = lineIndex;
			LineData line = LineManager.Current.GetLine(curLineIndex);
			int num = -1;
			float num2 = float.PositiveInfinity;
			for (int num3 = line.Path.Count - 2; num3 >= 0; num3--)
			{
				if (GameTools.HaveProjectOnSegment(line.Path[num3], line.Path[num3 + 1], curPos))
				{
					float num4 = GameTools.SquareDistancePointSegment(curPos, line.Path[num3], line.Path[num3 + 1]);
					if (num4 < num2)
					{
						num2 = num4;
						num = num3;
					}
				}
			}
			Vector3 projectOnLine = GameTools.GetProjectOnLine(line.Path[num], line.Path[num + 1], curPos);
			MonsterPathAnchor monsterPathAnchor = new MonsterPathAnchor(projectOnLine, num, line, curLineIndex);
			firstAnchor = new MonsterPathAnchor(curLineIndex, monsterPathAnchor.pathSegmentId, monsterPathAnchor.lenProgress);
			secondAnchor = new MonsterPathAnchor(curLineIndex, monsterPathAnchor.pathSegmentId, monsterPathAnchor.lenProgress);
			LineManager.Current.MoveMonsterAnchor(firstAnchor, -0.4f);
			LineManager.Current.MoveMonsterAnchor(secondAnchor, 0.4f);
		}

		public Vector3 GetCurPos()
		{
			return (firstAnchor.pos + secondAnchor.pos) * 0.5f + offset * offsetProportion;
		}
	}
}
