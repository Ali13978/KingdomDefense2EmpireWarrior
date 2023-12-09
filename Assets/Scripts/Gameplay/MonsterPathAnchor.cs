using UnityEngine;

namespace Gameplay
{
	public class MonsterPathAnchor
	{
		public int curLineIndex;

		public int pathSegmentId;

		public float lenProgress;

		private LineData lineData;

		public Vector3 pos => lineData.Path[pathSegmentId] + lineData.segmentForwards[pathSegmentId] * lenProgress;

		public MonsterPathAnchor()
		{
		}

		public MonsterPathAnchor(Vector3 posOnSegment, int pathSegmentId, LineData lineData, int curLineIndex)
		{
			this.curLineIndex = curLineIndex;
			this.lineData = lineData;
			this.pathSegmentId = pathSegmentId;
			lenProgress = (posOnSegment - lineData.Path[pathSegmentId]).magnitude;
		}

		public MonsterPathAnchor(int curLineIndex, int pathSegmentId, float lenProgress)
		{
			this.curLineIndex = curLineIndex;
			this.pathSegmentId = pathSegmentId;
			this.lenProgress = lenProgress;
			lineData = LineManager.Current.GetLine(curLineIndex);
		}

		public MonsterPathAnchor(MonsterPathAnchor pAnchor)
		{
			curLineIndex = pAnchor.curLineIndex;
			pathSegmentId = pAnchor.pathSegmentId;
			lenProgress = pAnchor.lenProgress;
			lineData = LineManager.Current.GetLine(curLineIndex);
		}
	}
}
