# UartOscilloscope(Uart�����ܪi��)

##�\�໡���G
�n�����Ҧ��������`(Normal)�Ҧ��P����(Debug)�Ҧ��C���`�Ҧ��U���ѤU�C�\��G
- �i����Uart�s�u�C
- �i�վ�Uart�s�u�]�w(�j�v�B�P��줸)�C
- �i�վ�@��ާ@�����t�m(Uart�����ܦr���j�p�B����B���鵥)�C
�����Ҧ��U���]�t���`�Ҧ������\��~�A�󴣨ѥH�U�B�~��T�i�氣�����ջP�վ�]�w�G
- ��ܤw�s�u(�i�ѵ{���ϥ�)��SerialPort(��C�q�T��)�ƶq�C
- ��ܡubutton1_Click�v�Ƶ{���۵{���}�l�B��ɤ����榸�ơA�i��ʭ��m(�k�s)�C
- ��ܡubutton2_Click�v�Ƶ{���۵{���}�l�B��ɤ����榸�ơA�i��ʭ��m(�k�s)�C
- ��ܡubutton3_Click�v�Ƶ{���۵{���}�l�B��ɤ����榸�ơA�i��ʭ��m(�k�s)�C
- ��ܡu�]�w_�ǿ�]�wToolStripMenuItem_Click�v�Ƶ{���۵{���}�l�B��ɤ����榸�ơA�i��ʭ��m(�k�s)�C
- ��ܡu�]�w_�����]�wToolStripMenuItem_Click�v�Ƶ{���۵{���}�l�B��ɤ����榸�ơA�i��ʭ��m(�k�s)�C
- ��ܡulist_SerialPort�v�Ƶ{���۵{���}�l�B��ɤ����榸�ơA�i��ʭ��m(�k�s)�C
- ��ܡucomport_DataReceived�v�Ƶ{���۵{���}�l�B��ɤ����榸�ơA�i��ʭ��m(�k�s)�C
- ��ܡuUart_comport_handle�v�Ƶ{���۵{���}�l�B��ɤ����榸�ơA�i��ʭ��m(�k�s)�C
- ��ܡuDisplayText�v�Ƶ{���۵{���}�l�B��ɤ����榸�ơA�i��ʭ��m(�k�s)�C
- �i�����comport_DataReceived�Ƶ{�������Uart�ǿ餧Buffer�j�p�C
- �i�����comport_DataReceived�Ƶ{�������Uart�ǿ餧Buffer���(ASCII�s�X��)�C
- ��ܿ��~��T�s�X(Error_Code)�C

##������T�G
- 2016.8.12(��)
	- �ŧiAnalysis_Graphic_Mode�R�A�����ܼơA����{�����R�Pø�Ϥ�k�C
	- �]�p�s�X�i�ѦҰƵ{���[�c�ϡC
	- DisplayText�Ƶ{����textBox1��ܸ�Ƥ�k��i�G
	textBox1.Text += System.Text.Encoding.ASCII.GetString(buffer);�אּtextBox1.AppendText(System.Text.Encoding.ASCII.GetString(buffer));
	- comport��Ʊ����B�z�Ƶ{�����Htry��k����invoke
- 2016.11.13(��)
	- �ŧiĵ�i�T�����O(Error_code_message)�A�ξ���~�T����T�C
					��ĵ�i�T�����O(Error_code_message)�إ�Error_010001_Message�BError_010001_Title�BError_010001_MessageBoxButton�BError_010001_MessageBoxIcon�|���R�A����
					���c�ƿ��~�T���A�إ�Error_message_struct(���~�T�����c)�A
					�B�N���~�T�����e�ʸ˩�Error_message_struct(���~�T�����c)���A�~���L�k���N�ק�A
					�t���~�T����ܤ��A�����I�sMessageBox.Show�A�ӬO��Error_code_message���O����Error_Message_Show�Ƶ{��������~�T�����
- 2016.12.23(��)	���s�R�W�M�׬�UartOscilloscope�A�äW�Ǧ�GitHub
- 2016.12.24(��)	�վ�R�W�Ŷ�(namespace)��UartOscilloscope