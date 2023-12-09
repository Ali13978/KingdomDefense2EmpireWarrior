using DG.Tweening;
using Middle;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay
{
	public class LineManager : MonoBehaviour
	{
		private int gateCount;

		[NonSerialized]
		public List<MapGateController> listGates = new List<MapGateController>();

		[NonSerialized]
		public List<ShadowRoad> shadowRoads = new List<ShadowRoad>();

		public Dictionary<int, LineData> linesData = new Dictionary<int, LineData>();

		public static LineManager Current
		{
			get;
			set;
		}

		private void Awake()
		{
			Current = this;
			AddMemberGates();
			AddMemberShadowRoads();
		}

		private void AddMemberGates()
		{
			MapGateController[] componentsInChildren = GetComponentsInChildren<MapGateController>();
			listGates = new List<MapGateController>(componentsInChildren);
			for (int i = 0; i < listGates.Count; i++)
			{
				listGates[i].Init();
				for (int j = 0; j < listGates[i].gates.Count; j++)
				{
					DOTweenPath component = listGates[i].gates[j].GetComponent<DOTweenPath>();
					LineData value = new LineData(component);
					int lineIndex = GetLineIndex(i, j);
					linesData.Add(lineIndex, value);
				}
			}
		}

		private void AddMemberShadowRoads()
		{
			ShadowRoad[] componentsInChildren = GetComponentsInChildren<ShadowRoad>();
			shadowRoads = new List<ShadowRoad>(componentsInChildren);
		}

		public LineData GetLine(int gate, int line)
		{
			gateCount = listGates.Count;
			if (gate >= gateCount || line >= Config.Instance.LineCount)
			{
				throw new IndexOutOfRangeException();
			}
			int lineIndex = GetLineIndex(gate, line);
			if (!linesData.TryGetValue(lineIndex, out LineData value))
			{
				DOTweenPath component = listGates[gate].gates[line].GetComponent<DOTweenPath>();
				value = new LineData(component);
				linesData.Add(lineIndex, value);
			}
			return value;
		}

		public LineData GetLine(int lineIndex)
		{
			int gate = lineIndex / 100;
			int line = lineIndex % 100;
			return GetLine(gate, line);
		}

		public int GetLineIndex(int gate, int line)
		{
			return gate * 100 + line;
		}

		public void FindNearestLine(Vector3 targetPos, out int lineIndex, out Vector3 offset, out MonsterPathAnchor anchorOnSegment)
		{
			int num = -1;
			int num2 = -1;
			float num3 = float.PositiveInfinity;
			foreach (KeyValuePair<int, LineData> linesDatum in linesData)
			{
				LineData value = linesDatum.Value;
				for (int num4 = value.Path.Count - 2; num4 >= 0; num4--)
				{
					if (GameTools.HaveProjectOnSegment(value.Path[num4], value.Path[num4 + 1], targetPos))
					{
						float num5 = GameTools.SquareDistancePointSegment(targetPos, value.Path[num4], value.Path[num4 + 1]);
						if (num5 < num3)
						{
							num3 = num5;
							num = linesDatum.Key;
							num2 = num4;
						}
					}
				}
			}
			lineIndex = num;
			LineData line = GetLine(lineIndex);
			Vector3 projectOnLine = GameTools.GetProjectOnLine(line.Path[num2], line.Path[num2 + 1], targetPos);
			offset = targetPos - projectOnLine;
			anchorOnSegment = new MonsterPathAnchor(projectOnLine, num2, line, lineIndex);
		}

		public void RequestMove(EnemyModel enemy, MonsterPathData monsterPathData, float moveDistance, bool shorteningOffset = false, float dt = 0f)
		{
			MoveMonsterAnchor(monsterPathData.firstAnchor, moveDistance);
			MoveMonsterAnchor(monsterPathData.secondAnchor, moveDistance);
			if (shorteningOffset)
			{
				monsterPathData.offsetProportion = Mathf.MoveTowards(monsterPathData.offsetProportion, 0f, dt);
			}
			enemy.transform.position = monsterPathData.GetCurPos();
			LineData lineData = linesData[monsterPathData.firstAnchor.curLineIndex];
			int num = lineData.segmentForwards.Length;
			if (monsterPathData.firstAnchor.pathSegmentId == num - 1 && monsterPathData.OnCompletePath != null && monsterPathData.firstAnchor.lenProgress >= lineData.segmentLengths[num - 1])
			{
				monsterPathData.OnCompletePath();
				monsterPathData.OnCompletePath = null;
			}
		}

		public void MoveMonsterAnchor(MonsterPathAnchor anchor, float moveDistance)
		{
			LineData lineData = linesData[anchor.curLineIndex];
			while (anchor.lenProgress + moveDistance < 0f && anchor.pathSegmentId > 0)
			{
				moveDistance += anchor.lenProgress;
				anchor.pathSegmentId--;
				anchor.lenProgress = lineData.segmentLengths[anchor.pathSegmentId];
			}
			while (anchor.lenProgress + moveDistance > lineData.segmentLengths[anchor.pathSegmentId] && anchor.pathSegmentId < lineData.segmentForwards.Length - 1)
			{
				moveDistance -= lineData.segmentLengths[anchor.pathSegmentId] - anchor.lenProgress;
				anchor.pathSegmentId++;
				anchor.lenProgress = 0f;
			}
			anchor.lenProgress += moveDistance;
		}
	}
}
