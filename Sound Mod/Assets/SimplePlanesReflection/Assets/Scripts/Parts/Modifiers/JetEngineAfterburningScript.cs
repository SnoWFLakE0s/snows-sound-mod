namespace Assets.SimplePlanesReflection.Assets.Scripts.Parts.Modifiers
{
    using UnityEngine;

    public partial class JetEngineAfterburningScript : MonoBehaviourProxyType<JetEngineAfterburningScript> {
        private static Property<MonoBehaviour> _partScript = CreateProperty<MonoBehaviour>("PartScript");

        private static Field<AudioSource> _engineAudioSource = CreateField<AudioSource>("_engineAudioSource");
        private static Field<ParticleSystem.EmissionModule> _smokeSystemEmission = CreateField<ParticleSystem.EmissionModule>("_smokeSystemEmission");
        private static Field<ParticleSystem.EmissionModule> _afterburningSmokeSystemEmission = CreateField<ParticleSystem.EmissionModule>("_afterburningSmokeSystemEmission");

        protected JetEngineAfterburningScript() {
        }

        public AudioSource EngineAudioSource {
            get {
                return this.Get(_engineAudioSource);
            }
        }

        public ParticleSystem.EmissionModule SmokeSystemEmission {
            get {
                return this.Get(_smokeSystemEmission);
            }
        }

        public ParticleSystem.EmissionModule AfterburningSmokeSystemEmission {
            get {
                return this.Get(_afterburningSmokeSystemEmission);
            }
        }

        public PartScript PartScript {
            get {
                return PartScript.Wrap(this.Get(_partScript));
            }
        }
    }
}