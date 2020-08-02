import * as axios from "../http.adapter"
import moment from "moment"
import { Link } from "react-router-dom"
import { Image } from "./BirdImage"
import { MapWrap } from "./MapWrap"
import { useState, useEffect } from "react"
import React from "react"


export const PhotoPage: React.FC<any> = props => {
    const [image, setImage] = useState<Image | null>(null)

    useEffect(() => {
        axios.get(`/api/gallery/photo/${props.match.params.id}`).then((res) => {
            const receivedImage = res.data.photo
            receivedImage.tags = [{title: receivedImage.caption, value: receivedImage.caption}]
            setImage(receivedImage)
        })
    }, [props])

    if (!image)
        return null

    return (
            <div className="body">

            {image.birdIds.map(id => <Link
                    key={id}
                    className="big-link" to={`/birds/${id}`}>
                    {image.caption} ({moment(image.dateTaken).format("YYYY MM DD")})
                </Link>,
            )}
                <section className="photo-flex-container">
                    <div className="flex-item photo-flex-element">
                        <img src={image.original} className="photo-page"></img>
                    </div>
                    <div className="flex-item">
                        <MapWrap asPopup={true} locationIdToShow={image.locationId}/>
                    </div>
                </section>
            </div>)
}
