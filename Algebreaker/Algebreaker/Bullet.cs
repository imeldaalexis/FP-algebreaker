using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Timer = System.Windows.Forms.Timer;

namespace Algebreaker
{
    //Kelas untuk bullet yang dipakai player
    public class Bullet
    {
        private const int _bulletSpeed = 10;
        private int _currentRowBullet;
        private Image _bulletImage = Image.FromFile(@"Assets\bullet.png");
        private PictureBox _bulletPicBox;
        private Timer _bulletTimer;
        public static int _maxAmmo = 10; //Dipakai untuk menentukan maxAmmo player
        public static int _currentAmmo = _maxAmmo; //Dibuat static, karena variabel ini bukan untuk single bullet, melainkan seluruh objek bullet yang pernah tercipta

        //Konstruktor
        public Bullet(Point gunPosition, int currentRow)
        {
            _bulletPicBox = new PictureBox
            {
                Size = new Size(32, 32),
                Location = gunPosition,
                Image = _bulletImage,
                SizeMode = PictureBoxSizeMode.StretchImage,
                BackColor = Color.Transparent
            };

            _bulletTimer = new Timer
            {
                Interval = 50
            };
            _bulletTimer.Tick += OnBulletMove;
            _bulletTimer.Start();

            _currentRowBullet = currentRow;
            
            Debug.WriteLine($"Current Ammo: {_currentAmmo}");
        }

        //Event handler method
        private void OnBulletMove(object sender, EventArgs e)
        {
            CekArahBulletDanTembak(_currentRowBullet);
        }

        public void CekArahBulletDanTembak(int currentPlayerRow)
        {
            switch (currentPlayerRow)
            {
                //Yang ada dalam case nya adalah animasi bullet bergerak

                //Kalau current row player terakhir
                //0 -> Menghadap ke atas
                case 0:
                    _bulletPicBox.Top += _bulletSpeed;
                    break;
                //1 -> Menghadap ke Bawah
                case 1:
                    _bulletPicBox.Top -= _bulletSpeed;
                    break;
                //2 -> Menghadap ke kiri
                case 2:
                    _bulletPicBox.Left -= _bulletSpeed;
                    break;
                //3 -> Menghadap ke kanan
                case 3:
                    _bulletPicBox.Left += _bulletSpeed;
                    break;
            }
        }

        public PictureBox GetPictureBox() => _bulletPicBox;
    }
}
