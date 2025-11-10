namespace UD_SucKhoe
{
    public partial class ProfilePage : ContentPage
    {
        private bool isEditing = false;

        public ProfilePage()
        {
            InitializeComponent();
            LoadUserProfile();
        }

        private void LoadUserProfile()
        {
            try
            {
                // Lấy thông tin từ Preferences
                string fullName = Preferences.Get("FullName", "Nguoi dung");
                string email = Preferences.Get("Email", "Chua cap nhat");
                string phone = Preferences.Get("Phone", "Chua cap nhat");
                string address = Preferences.Get("Address", "Chua cap nhat");
                string birthday = Preferences.Get("Birthday", "01/01/2000");
                string password = Preferences.Get("Password", "");
                string createdDate = Preferences.Get("CreatedDate", "Chua cap nhat");

                // Hiển thị trên header
                lblFullName.Text = fullName;

                // Hiển thị ở các field
                lblFullNameField.Text = fullName;
                entryFullName.Text = fullName;

                lblEmailField.Text = email;
                entryEmail.Text = email;

                lblPhoneField.Text = phone;
                entryPhone.Text = phone;

                lblAddressField.Text = address;
                entryAddress.Text = address;

                lblBirthdayField.Text = birthday;
                if (DateTime.TryParseExact(birthday, "dd/MM/yyyy", null,
                    System.Globalization.DateTimeStyles.None, out DateTime birthDate))
                {
                    dateBirthday.Date = birthDate;
                }

                entryPassword.Text = password;
                lblCreatedDate.Text = createdDate;
            }
            catch (Exception ex)
            {
                DisplayAlert("Loi", $"Khong the tai thong tin: {ex.Message}", "OK");
            }
        }

        private void OnEditFullNameClicked(object sender, EventArgs e)
        {
            lblFullNameField.IsVisible = !lblFullNameField.IsVisible;
            entryFullName.IsVisible = !entryFullName.IsVisible;
            if (entryFullName.IsVisible) entryFullName.Focus();
            ShowUpdateButton();
        }

        private void OnEditEmailClicked(object sender, EventArgs e)
        {
            lblEmailField.IsVisible = !lblEmailField.IsVisible;
            entryEmail.IsVisible = !entryEmail.IsVisible;
            if (entryEmail.IsVisible) entryEmail.Focus();
            ShowUpdateButton();
        }

        private void OnEditPhoneClicked(object sender, EventArgs e)
        {
            lblPhoneField.IsVisible = !lblPhoneField.IsVisible;
            entryPhone.IsVisible = !entryPhone.IsVisible;
            if (entryPhone.IsVisible) entryPhone.Focus();
            ShowUpdateButton();
        }

        private void OnEditAddressClicked(object sender, EventArgs e)
        {
            lblAddressField.IsVisible = !lblAddressField.IsVisible;
            entryAddress.IsVisible = !entryAddress.IsVisible;
            if (entryAddress.IsVisible) entryAddress.Focus();
            ShowUpdateButton();
        }

        private void OnEditBirthdayClicked(object sender, EventArgs e)
        {
            lblBirthdayField.IsVisible = !lblBirthdayField.IsVisible;
            dateBirthday.IsVisible = !dateBirthday.IsVisible;
            ShowUpdateButton();
        }

        private void OnEditPasswordClicked(object sender, EventArgs e)
        {
            lblPasswordField.IsVisible = !lblPasswordField.IsVisible;
            entryPassword.IsVisible = !entryPassword.IsVisible;
            if (entryPassword.IsVisible) entryPassword.Focus();
            ShowUpdateButton();
        }

        private void ShowUpdateButton()
        {
            isEditing = !lblFullNameField.IsVisible ||
                        !lblEmailField.IsVisible ||
                        !lblPhoneField.IsVisible ||
                        !lblAddressField.IsVisible ||
                        !lblBirthdayField.IsVisible ||
                        !lblPasswordField.IsVisible;

            btnUpdate.IsVisible = isEditing;
        }

        private async void OnUpdateButtonClicked(object sender, EventArgs e)
        {
            try
            {
                // Validate
                if (string.IsNullOrWhiteSpace(entryFullName.Text))
                {
                    await DisplayAlert("Loi", "Ho va ten khong duoc de trong!", "OK");
                    return;
                }

                if (string.IsNullOrWhiteSpace(entryEmail.Text) || !entryEmail.Text.Contains("@"))
                {
                    await DisplayAlert("Loi", "Email khong hop le!", "OK");
                    return;
                }

                // Lưu vào Preferences
                Preferences.Set("FullName", entryFullName.Text);
                Preferences.Set("Email", entryEmail.Text);
                Preferences.Set("Phone", entryPhone.Text);
                Preferences.Set("Address", entryAddress.Text);
                Preferences.Set("Birthday", dateBirthday.Date.ToString("dd/MM/yyyy"));

                if (!string.IsNullOrWhiteSpace(entryPassword.Text))
                {
                    Preferences.Set("Password", entryPassword.Text);
                }

                // TODO: Cập nhật vào database
                await UpdateToDatabase();

                // Cập nhật UI
                lblFullName.Text = entryFullName.Text;
                lblFullNameField.Text = entryFullName.Text;
                lblEmailField.Text = entryEmail.Text;
                lblPhoneField.Text = entryPhone.Text;
                lblAddressField.Text = entryAddress.Text;
                lblBirthdayField.Text = dateBirthday.Date.ToString("dd/MM/yyyy");

                // Ẩn Entry, hiện Label
                lblFullNameField.IsVisible = true;
                entryFullName.IsVisible = false;

                lblEmailField.IsVisible = true;
                entryEmail.IsVisible = false;

                lblPhoneField.IsVisible = true;
                entryPhone.IsVisible = false;

                lblAddressField.IsVisible = true;
                entryAddress.IsVisible = false;

                lblBirthdayField.IsVisible = true;
                dateBirthday.IsVisible = false;

                lblPasswordField.IsVisible = true;
                entryPassword.IsVisible = false;

                btnUpdate.IsVisible = false;
                isEditing = false;

                await DisplayAlert("Thanh cong", "Da cap nhat thong tin!", "OK");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Loi", $"Co loi: {ex.Message}", "OK");
            }
        }

        private async Task UpdateToDatabase()
        {
            // TODO: Thêm code cập nhật database ở đây
            // Ví dụ với SQLite:
            /*
            var userId = Preferences.Get("UserId", 0);
            var conn = new SQLiteConnection(App.DatabasePath);
            var user = conn.Get<User>(userId);
            user.FullName = entryFullName.Text;
            user.Email = entryEmail.Text;
            user.Phone = entryPhone.Text;
            user.Address = entryAddress.Text;
            user.Birthday = dateBirthday.Date.ToString("dd/MM/yyyy");
            if (!string.IsNullOrWhiteSpace(entryPassword.Text))
                user.Password = entryPassword.Text;
            conn.Update(user);
            */
            await Task.CompletedTask;
        }

        private async void OnBackButtonClicked(object sender, EventArgs e)
        {
            if (isEditing)
            {
                bool answer = await DisplayAlert(
                    "Xac nhan",
                    "Ban dang chinh sua. Huy thay doi?",
                    "Co",
                    "Khong"
                );

                if (!answer) return;
            }

            await Navigation.PopAsync();
        }
    }
}