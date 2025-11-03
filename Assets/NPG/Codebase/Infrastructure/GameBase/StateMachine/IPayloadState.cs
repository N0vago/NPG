using NPG.Codebase.Infrastructure.GameBase.StateMachine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.NPG.Codebase.Infrastructure.GameBase.StateMachine
{
	public interface IPayloadState<T> :IExitableState
	{
		void Enter(T payload);
	}
}
