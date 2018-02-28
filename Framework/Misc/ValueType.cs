public struct ValueType<T> where T : struct {
    private T m_value;
    public T value {
        get { return m_value; }
        set { ValueChanged(value); }
    }

    public System.Action<T> onValueChanged;

    public void SetValue(T newValue) {
        m_value = newValue;
    }

    private void ValueChanged(T newValue) {
        m_value = newValue;

        if (onValueChanged != null)
            onValueChanged(m_value);
    }
}
