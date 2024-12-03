module BookingForm

open System
open System.IO
open System.Drawing
open System.Windows.Forms
open TicketManager
open SeatManager

type BookingForm(ticketManager: TicketManager, seatManager: SeatManager, showtime: string, row: int, col: int) =
    member this.ShowBookingForm() =
        let bookingForm = new Form(Text = "Booking Details", Size = Size(300, 200))
        let nameLabel = new Label(Text = "Customer Name:", Location = Point(10, 10), AutoSize = true)
        let nameTextBox = new TextBox(Location = Point(120, 10), Width = 150)
        let submitButton = new Button(Text = "Confirm Booking", Location = Point(100, 100), AutoSize = true, AutoSizeMode = AutoSizeMode.GrowAndShrink)

        // Handle booking submission
        submitButton.Click.Add(fun _ ->
            let customerName = nameTextBox.Text
            if String.IsNullOrWhiteSpace(customerName) then
                MessageBox.Show("Customer name cannot be empty.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error) |> ignore
            else
                let ticketID = ticketManager.GenerateTicketID()
                let seat = $"Row {row + 1}, Col {col + 1}"
                try
                    ticketManager.SaveTicketToFile(ticketID, customerName, seat, showtime)
                    MessageBox.Show($"Booking Confirmed!\nTicket ID: {ticketID}") |> ignore
                    // Log success message
                    File.AppendAllText("log.txt", $"Booking confirmed for ticket: {ticketID}\n")
                    bookingForm.Close()
                with
                | ex ->
                    MessageBox.Show($"Failed to save booking: {ex.Message}") |> ignore
                    // Log the exception to a file for debugging purposes
                    File.AppendAllText("error_log.txt", $"Error during booking: {ex.Message}\n")
        )

        bookingForm.Controls.AddRange([| nameLabel; nameTextBox; submitButton |])
        bookingForm.ShowDialog() |> ignore