using UnityEngine;
using HarmonyLib;
using Photon.Pun;

namespace SuperSlipperyJellyfish
{
    internal class JellyfishPatch
    {
        private static Rigidbody GetBodypartRig(Character character, BodypartType bp)
        {
            return character.refs.ragdoll.partDict[bp].Rig;
        }

        [HarmonyPatch(typeof(SlipperyJellyfish), nameof(SlipperyJellyfish.Trigger))]
        [HarmonyPrefix]
        private static bool TriggerPatch(int targetID, SlipperyJellyfish __instance)
        {
            // Override the trigger to scale forces and optional volume without duplicating the original method body.
            Character component = PhotonView.Find(targetID)!.GetComponent<Character>();
            if (component == null)
            {
                Log.Write.LogWarning($"No Character found for target {targetID}; skipping slipperiness override.");
                return false;
            }

            Log.Write.LogInfo($"Running original code with {Base.multiplier.Value}x force multiplier");
            ApplyImpulses(component);
            ApplySlipEffects(__instance, component);
            return false;
        }

        private static void ApplyImpulses(Character component)
        {
            Rigidbody rightFoot = GetBodypartRig(component, BodypartType.Foot_R);
            Rigidbody leftFoot = GetBodypartRig(component, BodypartType.Foot_L);
            Rigidbody hip = GetBodypartRig(component, BodypartType.Hip);
            Rigidbody head = GetBodypartRig(component, BodypartType.Head);

            component.RPCA_Fall(2f);
            Vector3 lookUp = component.data.lookDirection_Flat + Vector3.up;
            rightFoot.AddForce(lookUp * 200f, ForceMode.Impulse);
            leftFoot.AddForce(lookUp * 200f, ForceMode.Impulse);
            hip.AddForce(Vector3.up * (1500f * Base.multiplier.Value), ForceMode.Impulse);
            head.AddForce(component.data.lookDirection_Flat * -300f, ForceMode.Impulse);
        }

        private static void ApplySlipEffects(SlipperyJellyfish instance, Character component)
        {
            component.refs.afflictions.AddStatus(CharacterAfflictions.STATUSTYPE.Poison, 0.05f, true);

            for (int i = 0; i < instance.slipSFX.Length; i++)
            {
                if (Base.volume.Value)
                    instance.slipSFX[i].settings.volume = Base.multiplier.Value;

                instance.slipSFX[i].Play(instance.transform.position);
            }
        }
    }
}
