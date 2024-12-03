module TicketManager
open System
open System.IO

type TicketManager() =
    member this.GenerateTicketID() = Guid.NewGuid().ToString()

    member this.SaveTicketToFile(ticketID: string, customerName: string, seat: string, showtime: string) =
        let ticketDetails = $"Ticket ID: {ticketID}\nCustomer: {customerName}\nSeat: {seat}\nShowtime: {showtime}\n\n"
        File.AppendAllText("bookings.txt", ticketDetails)