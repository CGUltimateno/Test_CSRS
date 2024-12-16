module BookingForm
open System
open System.Drawing
open System.Windows.Forms
open TicketManager

type BookingForm(ticketManager: TicketManager, showtime: string, row: int, col: int) =
    member this.ShowBookingForm() =
        let bookingForm = new Form(Text = "Booking Details", Size = Size(300, 200))
        let nameLabel = new Label(Text = "Customer Name:", Location = Point(10, 10), AutoSize = true)
        let nameTextBox = new TextBox(Location = Point(120, 10), Width = 150)
        let submitButton = new Button(Text = "Confirm Booking", Location = Point(100, 100), AutoSize = true)

        let validateCustomerName name =
            match name with
            | "" | null -> false
            | _ -> true

        let mutable bookingConfirmed = false

        let handleBooking () =
            let customerName = nameTextBox.Text
            if validateCustomerName customerName then
                let ticketID = ticketManager.GenerateTicketID()
                let seat = $"Row {row + 1}, Col {col + 1}"
                let ticket = { TicketID = ticketID; CustomerName = customerName; Seat = seat; Showtime = showtime }
                ticketManager.SaveTicket(ticket)
                MessageBox.Show($"Booking Confirmed!\nTicket ID: {ticketID}") |> ignore
                bookingConfirmed <- true
                bookingForm.Close()
            else
                MessageBox.Show("Customer name cannot be empty.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error) |> ignore

        submitButton.Click.Add(fun _ -> handleBooking())

        bookingForm.FormClosing.Add(fun _ -> bookingForm.DialogResult <- if bookingConfirmed then DialogResult.OK else DialogResult.Cancel)

        bookingForm.Controls.AddRange([| nameLabel; nameTextBox; submitButton |])
        bookingForm.ShowDialog() |> ignore
        bookingConfirmed