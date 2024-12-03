module CinemaForm
   open System
   open System.Drawing
   open System.Windows.Forms
   open TicketManager
   open SeatManager
   open BookingForm
 
   type CinemaForm(seatManager: SeatManager, ticketManager: TicketManager, showtimes: string[]) =
    let form = new Form(Text = "Cinema Seat Reservation", Size = Size(500, 500))
    let panel = new Panel(Dock = DockStyle.Fill, Location = Point(0, 100), Size = Size(500, 400))
    let showtimeDropdown = new ComboBox(Location = Point(10, 10), Width = 200)

    // Initialize dropdown and showtime layouts
    do
        form.Controls.Add(showtimeDropdown)
        form.Controls.Add(panel)
        showtimeDropdown.Items.AddRange(showtimes |> Array.map box)
        showtimeDropdown.SelectedIndex <- 0

        for showtime in showtimes do
            seatManager.InitializeShowtime(showtime)

    // Redraw seat layout for the selected showtime
    let redrawSeats showtime =
        panel.Controls.Clear()

        let handleSeatClick (row: int) (col: int) (button: Button) =
            if seatManager.IsSeatAvailable(showtime, row, col) then
                seatManager.ReserveSeat(showtime, row, col)
                button.BackColor <- Color.Red
                button.Enabled <- false
                let bookingForm = BookingForm(ticketManager, seatManager, showtime, row, col)
                bookingForm.ShowBookingForm()

        let seatLayout = seatManager.GetSeatLayout(showtime)
        let totalRows = Array2D.length1 seatLayout
        let totalCols = Array2D.length2 seatLayout
        let buttonWidth = 60
        let buttonHeight = 40
        let horizontalSpacing = 65
        let verticalSpacing = 45
        let totalWidth = totalCols * horizontalSpacing
        let totalHeight = totalRows * verticalSpacing
        let startX = (panel.Width - totalWidth) / 2
        let startY = (panel.Height - totalHeight) / 2

        for row in 0 .. totalRows - 1 do
            for col in 0 .. totalCols - 1 do
                let button = new Button(Text = $"R{row + 1}C{col + 1}", Size = Size(buttonWidth, buttonHeight), Location = Point(startX + col * horizontalSpacing, startY + row * verticalSpacing))
                button.BackColor <- if seatManager.IsSeatAvailable(showtime, row, col) then Color.Green else Color.Red
                button.Click.Add(fun _ -> handleSeatClick row col button)
                panel.Controls.Add(button)

    // Handle showtime selection change
    do showtimeDropdown.SelectedIndexChanged.Add(fun _ ->
        let selectedShowtime = showtimeDropdown.SelectedItem.ToString()
        redrawSeats selectedShowtime)

    member this.Show() =
        redrawSeats (showtimeDropdown.SelectedItem.ToString())
        Application.Run(form)