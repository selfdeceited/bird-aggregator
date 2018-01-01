import * as React from "react"
import * as Blueprint from "@blueprintjs/core";
import { Select, ISelectItemRendererProps  } from "@blueprintjs/labs";
import axios from 'axios';
import { Link } from 'react-router-dom'

const tempBird = {id: 42, name: "some bird", latin: "some latin text"};
export type Bird = typeof tempBird;

export interface NavbarProps { }
export interface NavbarState {
    flickr: string,
    github: string,
    owner: string,
    birds: Bird[],
    selectedBird: Bird
 }

const BirdSelect = Select.ofType<Bird>()

export default class Navbar extends React.Component<NavbarProps, NavbarState>  {
    constructor(props) {
        super(props);

        this.state = {
            flickr: "",
            github: "",
            owner: "",
            birds: [],
            selectedBird: { id: 0, name: "", latin: "" }
        };
    }

    componentDidMount() {
        axios.get(`/api/meta/flickr`).then(res => {
            const flickr = res.data;
            this.setState({ flickr });
        });

        axios.get(`/api/meta/github`).then(res => {
            const github = res.data;
            this.setState({ github });
        });

        axios.get(`/api/meta/user`).then(res => {
            const owner = res.data;
            this.setState({ owner });
        });

        axios.get(`/api/birds/`).then(res => {
            const birds = res.data;
            this.setState({ birds });

            const selectedBird = birds[0];
            this.setState({ selectedBird });
        });
    }

    render() {
        return  (
<nav className="pt-navbar pt-fixed-top">
    <div className="pt-navbar-group pt-align-left">
    <div className="logo"></div>
    <Link to="/" title="Main page" className="pt-navbar-heading"><b>{this.state.owner}</b></Link>
    <span className="pt-navbar-divider"></span>
    <Link to="/map" title="Map" role="button" className="pt-button pt-minimal pt-icon-map small-margin">Map</Link>
    <Link to="/triplist" title="Trips" role="button" className="pt-button pt-minimal pt-icon-torch small-margin">Trips</Link>
    <Link to="/lifelist" title="Life List" role="button" className="pt-button pt-minimal pt-icon-numbered-list small-margin">Life List</Link>
    
    <span className="pt-navbar-divider"></span>
    <div className="bird-info-select">
        <div className="inline-block">Find specific bird: &nbsp;</div>
        <BirdSelect
            items={this.state.birds}
            noResults={<Blueprint.MenuItem disabled text="No results." />}
            itemPredicate={this.filterBird}
            itemRenderer={this.renderBird}
            onItemSelect={this.onSelect}
        >
            <Blueprint.Button text={this.state.selectedBird.name} rightIconName="double-caret-vertical" />
        </BirdSelect>
        <span className="small-space"></span>
        <Link to={"/birds/" + this.state.selectedBird.id} role="button" className="pt-button pt-minimal pt-icon-arrow-right"></Link>
    </div>
  </div>
  <div className="pt-navbar-group pt-align-right">
    <a role="button" className="pt-button pt-minimal pt-icon-git-repo" href={this.state.github}>GitHub</a>
    <a role="button" className="pt-button pt-minimal pt-icon-group-objects" href={this.state.flickr}>Flickr</a>
  </div>
</nav>)
    }

    private filterBird(query: string, bird: Bird, index: number) {
        return `${bird.name.toLowerCase()} ${bird.latin.toLowerCase()}`.indexOf(query.toLowerCase()) >= 0;
    }

    private renderBird = ({ handleClick, item: bird, isActive }: ISelectItemRendererProps<Bird>) => (
    <Blueprint.MenuItem
        className=""
        key={bird.name}
        label={bird.latin}
        onClick={handleClick}
        text={bird.name}
    />
    );

    private onSelect = (bird: Bird) => {
        const selectedBird = bird; 
        this.setState({selectedBird});
    }
}