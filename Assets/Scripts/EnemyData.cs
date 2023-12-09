namespace Parameter
{
	public struct EnemyData
	{
		public int wave;

		public int time;

		public int id;

		public bool isLastInWave;

		public int gate;

		public int formationId;

		public int minDifficulty;

		public EnemyData(int time, int id, bool isLastInWave, int gate)
		{
			wave = 0;
			this.time = time;
			this.id = id;
			this.isLastInWave = isLastInWave;
			this.gate = gate;
			formationId = 0;
			minDifficulty = 0;
		}

		public EnemyData(int wave, int time, int id, bool isLastInWave, int gate, int formationId, int minDifficulty)
		{
			this = new EnemyData(time, id, isLastInWave, gate);
			this.formationId = formationId;
			this.minDifficulty = minDifficulty;
			this.wave = wave;
		}
	}
}
