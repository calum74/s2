#pragma once

namespace S2
{
	class Generator;

	enum GeneratorStatus
	{
		Available,
		Open,
		InUse,
		Disconnected,
		PermissionError
	};

	enum class FrequencyScale { Low, High, Unknown };

	class Channel
	{
	public:

	private:
		// The current state.
		// Only update the state if
		double amplitude, frequency, duty;
		BuiltinWaveform waveform;

		virtual void Amplitude(double value) = 0;
		virtual void Frequency(double value) = 0;
		virtual void Waveform(BuiltinWaveform value) = 0;
		virtual void Waveform(const WaveData & data)=0;
		virtual void Duty(double p) = 0;
		virtual void Output(bool b) = 0;
		virtual void Offset(double p) = 0;

		//virtual void Sync() = 0;
		//virtual void Phase(int p) = 0;
		// !! Sync and phase ?? 
	};

	class SingleChannel : public Channel
	{
	public:
		SingleChannel(Generator & generator, int id);
		void Amplitude(double value);
		void Frequency(double value);
		void Waveform(BuiltinWaveform value);
		void Waveform(const WaveData & data);
		void Duty(double p);
		void Output(bool b);
		void Offset(double p);

	private:
		SingleChannel(const SingleChannel&) = delete;
		Generator & generator;
		int channel;
	};

	class InvertAndSync : public Channel
	{
	public:
		InvertAndSync(Generator & generator);
		InvertAndSync(const InvertAndSync&) = delete;


		void Amplitude(double value);
		void Frequency(double value);
		void Waveform(BuiltinWaveform value);
		void Waveform(const WaveData & data);
		void Duty(double p);
		void Output(bool b);
		void Offset(double p);

	private:
		Generator & generator;
		void Sync();
	};

	class Generator : public Device
	{
	public:
		Generator(int id, const std::string & filename);
		Generator(const Generator & other);
		Generator & operator=(const Generator & other);

		Channel & GetChannel(int c);

		GeneratorStatus GetStatus();

		friend class SingleChannel;
		friend class InvertAndSync;


		void Open(Devices&, StreamFactory&);
		// bool Verbose(bool b); // Change verbose mode

		void Send(const char * buffer);

		void Sync(bool on);
		void Phase(int channel, int degrees);

	private:
		void Amplitude(int channel, double value);
		void Output(int channel, bool value);
		void Frequency(int channel, double value);
		void Waveform(int channel, BuiltinWaveform value);
		void Waveform(int channel, const WaveData & data);
		void Duty(int channel, double percent);

		// Offset is a number between -1 and +1. 0 means offset =0%
		// Normally just use 0
		void Offset(int channel, double offset);

		void Send(int command, int channel, int data);

	private:
		InvertAndSync channel0;
		SingleChannel channel1, channel2;
		void FrequencyScaling(int channel, FrequencyScale);
		bool verbose;
		FrequencyScale frequencyScaling[2];
		enum SyncStatus { On, Off, Unknown } sync;
	};

	class DummyGenerator : public Stream
	{
	public:
		DummyGenerator(bool verbose);
		int Read(char * buffer, int size);
		int Write(const char * buffer, int size);
	private:
		bool verbose;
	};
}
