namespace GooglePlayGames.BasicApi.Multiplayer
{
	public class Invitation
	{
		public enum InvType
		{
			RealTime,
			TurnBased,
			Unknown
		}

		private InvType mInvitationType;

		private string mInvitationId;

		private Participant mInviter;

		private int mVariant;

		public InvType InvitationType => mInvitationType;

		public string InvitationId => mInvitationId;

		public Participant Inviter => mInviter;

		public int Variant => mVariant;

		internal Invitation(InvType invType, string invId, Participant inviter, int variant)
		{
			mInvitationType = invType;
			mInvitationId = invId;
			mInviter = inviter;
			mVariant = variant;
		}

		public override string ToString()
		{
			return $"[Invitation: InvitationType={InvitationType}, InvitationId={InvitationId}, Inviter={Inviter}, Variant={Variant}]";
		}
	}
}
